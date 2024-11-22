using Microsoft.EntityFrameworkCore;
using WPRProject_1A_2.Modellen.Abonnement;
using WPRProject_1A_2.Modellen.Voertuigmodellen;
using WPRProject_1A_2.Modellen.Betaling;
using WPRProject_1A_2.Modellen.Accounts;


namespace WPRProject_1A_2;

public class CarAndAllContext : DbContext
{
    public DbSet<Abonnement> Abonnementen { get; set; }
    public DbSet<Adres> Adressen { get; set; }
    public DbSet<Voertuig> Voertuigen { get; set; }
    public DbSet<Betaling> Betalingen { get; set; }
    public DbSet<Account> Accounts { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {}
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder) 
    {
        builder.UseSqlite("Data Source = CarAndAllContext.db");
    }
}