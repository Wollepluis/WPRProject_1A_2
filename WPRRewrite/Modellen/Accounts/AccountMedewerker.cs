using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountMedewerker : Account
{
    protected AccountMedewerker() { }
    protected AccountMedewerker(string email, string wachtwoord) 
        : base(email, wachtwoord) { }
}