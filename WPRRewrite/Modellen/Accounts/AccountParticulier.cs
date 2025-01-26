using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountParticulier : Account
{
    public AccountParticulier(string email, string wachtwoord, string naam, int adresId, int telefoonnummer, IPasswordHasher<Account> passwordHasher, CarAndAllContext context)
        : base(passwordHasher, context)
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

    //Tijdelijk
    public string? AccountType { get; set; } = "Particulier";
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
}