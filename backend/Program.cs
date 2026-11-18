using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;

namespace WebApiApp;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        var connection = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connection));

        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        await app.RunAsync();
    }
}
