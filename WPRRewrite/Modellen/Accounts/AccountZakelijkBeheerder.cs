using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public class AccountZakelijkBeheerder : AccountZakelijk
{
    public AccountZakelijkBeheerder(string email, string wachtwoord, int bedrijfId ,IPasswordHasher<Account> passwordHasher)
        : base(passwordHasher)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        BedrijfId = bedrijfId;
    }

    public AccountZakelijkBeheerder()
    {
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