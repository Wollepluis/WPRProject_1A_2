using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountParticulier(IPasswordHasher<Account> passwordHasher) : Account(passwordHasher)
{
    public int ParticulierAccountId { get; set; }
    public string Naam { get; set; }
    public int ParticulierAdres { get; set; }
    public int Telefoonnummer { get; set; }
    public int Account { get; set; }

    public override void UpdateAccount(IAccount updatedAccount)
    {
        var particulierAccount = (AccountParticulier)updatedAccount;
        
        Email = particulierAccount.Email;
        Wachtwoord = particulierAccount.Wachtwoord;
        Naam = particulierAccount.Naam;
        ParticulierAdres = particulierAccount.ParticulierAdres;
        Telefoonnummer = particulierAccount.Telefoonnummer;
    }

    public override PasswordVerificationResult WachtwoordVerifieren(string password)
    {
        return passwordHasher.VerifyHashedPassword(this, Wachtwoord, password);
    }
}