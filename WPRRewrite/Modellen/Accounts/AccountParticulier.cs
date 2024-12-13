using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountParticulier : Account
{
    public AccountParticulier(string email, string wachtwoord, string naam, int adresId, int telefoonnummer, IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        Naam = naam;
        AdresId = adresId;
        Telefoonnummer = telefoonnummer;
        Reserveringen = new List<Reservering>();
    }

    public AccountParticulier()
    {
    }

    public string Naam { get; set; }
    public int AdresId { get; set; }
    [ForeignKey("AdresId")]
    public Adres Adres { get; set; }
    public int Telefoonnummer { get; set; }
    private List<Reservering> Reserveringen;

    public override void UpdateAccount(IAccount updatedAccount)
    {
        var particulierAccount = (AccountParticulier)updatedAccount;
        
        Email = particulierAccount.Email;
        Wachtwoord = particulierAccount.Wachtwoord;
        Naam = particulierAccount.Naam;
        AdresId = particulierAccount.AdresId;
        Telefoonnummer = particulierAccount.Telefoonnummer;
    }
    public override PasswordVerificationResult WachtwoordVerify(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Wachtwoord mag niet null of leeg zijn", nameof(password));
        }

        if (string.IsNullOrEmpty(this.Wachtwoord))
        {
            throw new InvalidOperationException("Het opgeslagen wachtwoord is null of leeg.");
        }

        try
        {
            return PasswordHasher.VerifyHashedPassword(this, Wachtwoord, password);
        }
        catch (Exception ex)
        {
            // Voeg logging toe om de fout verder te analyseren
            Console.WriteLine($"Fout bij wachtwoordverificatie: {ex.Message}");
            throw;
        }
    }

}