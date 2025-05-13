using Microsoft.AspNetCore.SignalR;
using MaxsMusicQuiz.Backend.Services.Interfaces;
using MaxsMusicQuiz.Backend.Models.Entities;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MaxsMusicQuiz.Backend.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameService _gameService;
        private readonly IUserService _userService;
        private readonly ILogger<GameHub> _logger;

        // Dictionary to track connection IDs to user IDs for each game lobby
        // Key: JoinCode, Value: Dictionary of ConnectionId to UserId
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> _lobbyConnections = 
            new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();

        public GameHub(IGameService gameService, IUserService userService, ILogger<GameHub> logger)
        {
            _gameService = gameService;
            _userService = userService;
            _logger = logger;
        }

        // Join a game lobby
        public async Task JoinLobby(string joinCode, int userId)
        {
            try
            {
                // Get the game by join code
                var quizGame = await _gameService.GetGameByJoinCodeAsync(joinCode);
                if (quizGame == null)
                {
                    await Clients.Caller.SendAsync("LobbyError", "Game not found.");
                    return;
                }

                // Get user from the user service
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    await Clients.Caller.SendAsync("LobbyError", "User not found.");
                    return;
                }

                // Add the connection to the SignalR group for this game
                await Groups.AddToGroupAsync(Context.ConnectionId, joinCode);

                // Track the connection in our dictionary
                var lobbyConnections = _lobbyConnections.GetOrAdd(joinCode, 
                    _ => new ConcurrentDictionary<string, int>());
                lobbyConnections[Context.ConnectionId] = userId;

                // Create a player object for SignalR communication
                var player = new
                {
                    userId = user.Id,
                    username = user.Username,
                    profilePictureUrl = user.ProfilePictureUrl
                };

                // Get all existing players in the game
                var allPlayers = quizGame.QuizGameUsers
                    .Select(qgu => new
                    {
                        userId = qgu.UserId,
                        username = qgu.User.Username,
                        profilePictureUrl = qgu.User.ProfilePictureUrl
                    })
                    .ToList();

                // Send the complete player list to the joining player
                await Clients.Caller.SendAsync("CurrentPlayers", allPlayers);

                // Notify other players in the lobby that a new player has joined
                await Clients.GroupExcept(joinCode, Context.ConnectionId)
                    .SendAsync("PlayerJoined", player);

                _logger.LogInformation($"User {user.Username} (ID: {userId}) joined lobby {joinCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when joining lobby {joinCode} for user ID {userId}");
                await Clients.Caller.SendAsync("LobbyError", "An error occurred when joining the lobby.");
            }
        }

        // Manually leave a game lobby
        public async Task LeaveLobby(string joinCode)
        {
            try
            {
                await RemoveFromLobby(joinCode, Context.ConnectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when leaving lobby {joinCode}");
            }
        }

        // Helper method to remove a player from a lobby
        private async Task RemoveFromLobby(string joinCode, string connectionId)
        {
            // If this lobby exists in our tracking dictionary
            if (_lobbyConnections.TryGetValue(joinCode, out var connections))
            {
                // If this connection exists in the lobby
                if (connections.TryRemove(connectionId, out var userId))
                {
                    // If the lobby is now empty, remove it from tracking
                    if (connections.IsEmpty)
                    {
                        _lobbyConnections.TryRemove(joinCode, out _);
                    }

                    // Notify others that the player has left
                    await Clients.GroupExcept(joinCode, connectionId)
                        .SendAsync("PlayerLeft", userId);

                    _logger.LogInformation($"User ID {userId} left lobby {joinCode}");
                }
            }

            // Remove from SignalR group
            await Groups.RemoveFromGroupAsync(connectionId, joinCode);
        }

        // Handle disconnections
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Find all lobbies this connection is in
            foreach (var lobby in _lobbyConnections)
            {
                string joinCode = lobby.Key;
                var connections = lobby.Value;

                // If this connection is in this lobby, remove it
                if (connections.ContainsKey(Context.ConnectionId))
                {
                    await RemoveFromLobby(joinCode, Context.ConnectionId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Send a chat message in the lobby
        public async Task SendLobbyMessage(string joinCode, string message)
        {
            if (_lobbyConnections.TryGetValue(joinCode, out var connections) &&
                connections.TryGetValue(Context.ConnectionId, out var userId))
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user != null)
                {
                    await Clients.Group(joinCode).SendAsync("ReceiveLobbyMessage", new
                    {
                        userId = user.Id,
                        username = user.Username,
                        message = message,
                        timestamp = DateTime.UtcNow
                    });
                }
            }
        }

        // For the host to broadcast game state updates
        public async Task UpdateGameState(string joinCode, string gameState)
        {
            await Clients.Group(joinCode).SendAsync("GameStateUpdated", gameState);
        }
    }
}