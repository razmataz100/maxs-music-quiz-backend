namespace MaxsMusicQuiz.Backend.Extensions;

public static class PipelineExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowLocalhost5173");
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
       
        return app;
    }
}