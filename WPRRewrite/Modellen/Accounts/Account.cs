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

    public static Account MaakAccount(string accountType, string email, string wachtwoord,  int nummer, string? naam, int? adresId)
    {
        return accountType switch
        {
            "Particulier" => new AccountParticulier(
                email,
                wachtwoord,
                naam,
                nummer,
                (int) adresId
            ),
            "ZakelijkBeheerder" => new AccountZakelijkBeheerder(
                email,
                wachtwoord,
                nummer
            ),
            "ZakelijkHuurder" => new AccountZakelijkHuurder(
                email,
                wachtwoord,
                nummer
            ),
            "Frontoffice" => new AccountMedewerkerFrontoffice(
                email,
                wachtwoord
            ),
            "Backoffice" => new AccountMedewerkerBackoffice(
                email,
                wachtwoord
            ),
            _ => throw new ArgumentException($"Onbekend account type: {accountType}")
        };
    }
}