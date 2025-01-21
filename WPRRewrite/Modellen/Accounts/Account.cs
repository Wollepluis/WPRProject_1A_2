using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Modellen.Accounts;

public abstract class Account : IAccount
{
    protected readonly IPasswordHasher<Account> PasswordHasher = new PasswordHasher<Account>();

    public int AccountId { get; set; }
    [MaxLength(255)] public string Email { get; set; }
    [MaxLength(255)] public string Wachtwoord { get; set; }
    public string AccountType { get; set; }
    
    protected Account() { }
    protected Account(string email, string wachtwoord)
    {
        Email = email;
        Wachtwoord = PasswordHasher.HashPassword(this, wachtwoord);
    }
    
    public abstract void UpdateAccount(AccountDto account);

    public PasswordVerificationResult VerifieerWachtwoord(string wachtwoord)
    {
        return PasswordHasher.VerifyHashedPassword(this, Wachtwoord, wachtwoord);
    }

    public static Account MaakAccount(AccountDto gegevens)
    {
        return gegevens.AccountType switch
        {
            "Particulier" => new AccountParticulier(
                gegevens.Email, 
                gegevens.Wachtwoord, 
                gegevens.Naam,
                gegevens.Nummer, 
                gegevens.AdresId
            ),
            "ZakelijkBeheerder" => new AccountZakelijkBeheerder(
                gegevens.Email, 
                gegevens.Wachtwoord, 
                gegevens.Nummer
            ),
            "ZakelijkHuurder" => new AccountZakelijkHuurder(
                gegevens.Email, 
                gegevens.Wachtwoord, 
                gegevens.Nummer
            ),
            "Frontoffice" => new AccountMedewerkerFrontoffice(
                gegevens.Email, 
                gegevens.Wachtwoord
            ),
            "Backoffice" => new AccountMedewerkerBackoffice(
                gegevens.Email, 
                gegevens.Wachtwoord
            ),
            _ => throw new ArgumentException($"Onbekend account type: {gegevens.AccountType}")
        };
    }
}