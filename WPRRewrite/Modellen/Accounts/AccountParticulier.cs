using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountParticulier : Account
{
    public AccountParticulier(IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        
    }

    private AccountParticulier()
    {
        
    }
    
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
    public override PasswordVerificationResult WachtwoordVerify(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Wachtwoord mag niet null of leeg zijn", nameof(password));
        }

        return PasswordHasher.VerifyHashedPassword(this, Wachtwoord, password);
    }
}