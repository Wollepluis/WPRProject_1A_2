using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;

namespace WPRRewrite;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<CarAndAllContext>(options =>
            options.UseSqlServer(@"Server=WOLLEPLUISPC\SQLExpress;" +
                                 "Database=CarAndAll;" +
                                 "Integrated Security=True;" +
                                 "TrustServerCertificate=True"
                                 ));
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IAdresService, AdresService>();


        var app = builder.Build();
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

