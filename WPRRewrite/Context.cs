using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<Voertuig> Voertuigen { get; set; }
    public DbSet<Adres> Adressen { get; set; }
    public DbSet<Bedrijf> Bedrijven { get; set; }
    public DbSet<Abonnement> Abonnementen { get; set; }
    public DbSet<Account> Accounts { get; set; }
   
    public DbSet<Reservering> Reserveringen { get; set; }
    public DbSet<Reparatie> Reparaties { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Account>()
            .HasDiscriminator<string>("AccountType")
            .HasValue<AccountParticulier>("ParticulierAccount")
            .HasValue<AccountMedewerkerFrontoffice>("FrontofficeAccount")
            .HasValue<AccountMedewerkerBackoffice>("BackofficeAccount")
            .HasValue<AccountZakelijk>("ZakelijkAccount")
            .HasValue<AccountZakelijkBeheerder>("ZakelijkBeheerder")
            .HasValue<AccountZakelijkHuurder>("ZakelijkHuurder");
        
        builder.Entity<Voertuig>()
            .HasDiscriminator<string>("VoertuigType")
            .HasValue<Auto>("Auto")
            .HasValue<Camper>("Camper")
            .HasValue<Caravan>("Caravan");

        builder.Entity<Abonnement>()
            .HasDiscriminator<string>("AbonnementType")
            .HasValue<Abonnement>("BaseAbonnement")
            .HasValue<PayAsYouGo>("PayAsYouGo")
            .HasValue<UpFront>("UpFront");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        if (builder.IsConfigured) return;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseSqlServer(connectionString);
    }
}