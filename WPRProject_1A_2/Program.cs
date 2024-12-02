using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WPRProject_1A_2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var app = builder.Build();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Allowvite",
                builder => builder
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        app.UseCors("Allowvite");

        builder.WebHost.ConfigureKestrel(options =>
        {
        options.ListenLocalhost(5000);
        });

// Voeg services toe aan de DI-container
        builder.Services.AddControllers(); // Nodig om controllers zoals AdresController te ondersteunen
        builder.Services.AddEndpointsApiExplorer(); // Voor Swagger
        builder.Services.AddSwaggerGen(); // Swagger configuratie

// Middleware pipeline configureren
        if (app.Environment.IsDevelopment())
        {
            // Gebruik Swagger alleen in de ontwikkelomgeving
            app.UseSwagger();
            app.UseSwaggerUI(); // Swagger UI beschikbaar maken op /swagger
        }

        app.UseHttpsRedirection(); // Voor beveiligde verbindingen

        app.UseAuthorization(); // Voor eventuele authenticatie/authorisatie (indien nodig)

// Registreer de controllers
        app.MapControllers(); // Dit zorgt ervoor dat AdresController werkt

        app.Run();
    }
}