using Microsoft.EntityFrameworkCore;
using WPRRewrite.Enums;
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
            .HasDiscriminator<AccountTypeEnum>("AccountType")
            .HasValue<AccountParticulier>(AccountTypeEnum.Particulier)
            .HasValue<AccountMedewerkerFrontoffice>(AccountTypeEnum.Frontoffice)
            .HasValue<AccountMedewerkerBackoffice>(AccountTypeEnum.Backoffice)
            .HasValue<AccountZakelijk>(AccountTypeEnum.Zakelijk)
            .HasValue<AccountZakelijkBeheerder>(AccountTypeEnum.ZakelijkBeheerder)
            .HasValue<AccountZakelijkHuurder>(AccountTypeEnum.ZakelijkHuurder);
        
        builder.Entity<Account>()
            .Property(e => e.AccountType)
            .HasConversion(
                v => v.ToString(),
                v => (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum), v)
            );
        
        builder.Entity<Voertuig>()
            .HasDiscriminator<VoertuigTypeEnum>("VoertuigType")
            .HasValue<Auto>(VoertuigTypeEnum.Auto)
            .HasValue<Camper>(VoertuigTypeEnum.Camper)
            .HasValue<Caravan>(VoertuigTypeEnum.Caravan);
        
        builder.Entity<Voertuig>()
            .Property(e => e.VoertuigType)
            .HasConversion(
                v => v.ToString(),
                v => (VoertuigTypeEnum)Enum.Parse(typeof(VoertuigTypeEnum), v)
            );
        
        builder.Entity<Voertuig>()
            .Property(e => e.VoertuigStatus)
            .HasConversion(
                v => v.ToString(),
                v => (VoertuigStatusEnum)Enum.Parse(typeof(VoertuigStatusEnum), v)
            );

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