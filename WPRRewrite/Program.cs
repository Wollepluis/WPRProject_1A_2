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

        builder.Services.AddDbContext<CarAndAllContext>(options =>
            options.UseSqlServer(@"Server=tcp:wprproject.database.windows.net,1433;Initial Catalog=CarAndAll;Persist Security Info=False;User ID=BoterhamZakje;Password=Banaan123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
                                 
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()                    
                    .AllowAnyMethod();                   
            });
        });
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
        builder.Services.AddScoped<IAdresService, AdresService>();
        builder.Services.AddScoped<EmailSender>();
        
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

