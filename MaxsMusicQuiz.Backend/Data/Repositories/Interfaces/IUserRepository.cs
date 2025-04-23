using MaxsMusicQuiz.Backend.Data.Entities;

namespace MaxsMusicQuiz.Backend.Data.Repositories.Interfaces;

public interface IUserRepo
{
    User? GetById(int userId);
}