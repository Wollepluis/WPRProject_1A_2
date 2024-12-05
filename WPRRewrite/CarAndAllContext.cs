﻿using Microsoft.EntityFrameworkCore;
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

        builder.Entity<AccountParticulier>().HasBaseType<Account>();
        builder.Entity<AccountZakelijk>().HasBaseType<Account>();
        builder.Entity<AccountMedewerker>().HasBaseType<Account>();
        builder.Entity<AccountMedewerkerFrontoffice>().HasBaseType<AccountMedewerker>();
        builder.Entity<AccountMedewerkerBackoffice>().HasBaseType<AccountMedewerker>();
        builder.Entity<AccountZakelijkBeheerder>().HasBaseType<AccountZakelijk>();
        builder.Entity<AccountZakelijkHuurder>().HasBaseType<AccountZakelijk>();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer(@"Server=WOLLEPLUISPC\SQLExpress;" +
                             "Database=CarAndAll;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True");
    }

    public DbSet<Voertuig> Voertuigen { get; set; }
    public DbSet<Adres> Adressen { get; set; }
    public DbSet<Bedrijf> Bedrijven { get; set; }
    
    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<AccountZakelijk> ZakelijkAccounts { get; set; }
    public DbSet<AccountZakelijkBeheerder> ZakelijkBeheerderAccounts { get; set; }
    public DbSet<AccountZakelijkHuurder> ZakelijkHuurderAccounts { get; set; }
    
    public DbSet<AccountMedewerker> MedewerkerAccounts { get; set; }
    public DbSet<AccountMedewerkerFrontoffice> FrontofficeAccounts { get; set; }
    public DbSet<AccountMedewerkerBackoffice> BackofficeAccounts { get; set; }
    public DbSet<AccountParticulier> ParticulierAccounts { get; set; }
}