using System.Net.Mail;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var environment = builder.Environment.EnvironmentName;
        
        if(environment == "Development")
        {
            builder.Services.AddDbContext<CarAndAllContext>(options =>
                options.UseSqlServer(@"Server=tcp:carandalla.database.windows.net,1433;Initial Catalog=CarAndAllA;Persist Security Info=False;User ID=CarAndAll;Password=MelleWessels1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
        }
        else
        {
            builder.Services.AddDbContext<CarAndAllContext>(options =>
                options.UseSqlServer(@"Server=LaptopMorris\SQLEXPRESS;Database=CarandallTest;Trusted_Connection=True;TrustServerCertificate=True"));
        }
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()                    
                    .AllowAnyMethod();                   
            });
        });
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
        builder.Services.AddScoped<IAdresService, AdresService>();
        builder.Services.AddScoped<EmailSender>();
        builder.Services.AddScoped<SmtpClient>();
        
        var app = builder.Build();
        app.UseCors("AllowAll");
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

