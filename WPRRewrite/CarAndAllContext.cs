using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite;

public class CarAndAllContext : DbContext
{
    public CarAndAllContext(DbContextOptions<CarAndAllContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Account>()
            .HasDiscriminator<string>("AccountType")
            .HasValue<Account>("BaseAccount")
            .HasValue<AccountParticulier>("ParticulierAccount")
            .HasValue<AccountMedewerker>("MedewerkerAccount")
            .HasValue<AccountMedewerkerFrontoffice>("FrontofficeAccount")
            .HasValue<AccountMedewerkerBackoffice>("BackofficeAccount")
            .HasValue<AccountZakelijk>("ZakelijkAccount")
            .HasValue<AccountZakelijkBeheerder>("ZakelijkBeheerder")
            .HasValue<AccountZakelijkHuurder>("ZakelijkHuurder");

        builder.Entity<Voertuig>()
            .HasDiscriminator<string>("VoertuigType")
            .HasValue<Voertuig>("BaseVoertuig")
            .HasValue<Auto>("Auto")
            .HasValue<Camper>("Camper")
            .HasValue<Caravan>("Caravan");

        builder.Entity<Abonnement>()
            .HasDiscriminator<String>("AbonnementType")
            .HasValue<Abonnement>("BaseAbonnement")
            .HasValue<PayAsYouGo>("PayAsYouGo")
            .HasValue<UpFront>("UpFront");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer(@"Server=WOLLEPLUISLAPTO\SQLExpress;" +
                             "Database=CarAndAll9;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True");
    }

    public DbSet<Voertuig> Voertuigen { get; set; }
    public DbSet<Adres> Adressen { get; set; }
    public DbSet<Bedrijf> Bedrijven { get; set; }
    public DbSet<Abonnement> Abonnementen { get; set; }
    public DbSet<Account> Accounts { get; set; }
   
    public DbSet<Reservering> Reserveringen { get; set; }
    public DbSet<Reparatie> Reparaties { get; set; }
}