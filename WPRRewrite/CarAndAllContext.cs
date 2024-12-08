using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen;
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
            .HasValue<AccountZakelijkBeheerder>("MedewerkerAccount")
            .HasValue<AccountZakelijkHuurder>("MedewerkerAccount");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer(@"Server=WOLLEPLUISPC\SQLExpress;" +
                             "Database=CarAndAll;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True");
    }

    public DbSet<IVoertuig> Voertuigen { get; set; }
    public DbSet<Adres> Adressen { get; set; }
    public DbSet<Bedrijf> Bedrijven { get; set; }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Reservering> Reserveringen { get; set; }
}