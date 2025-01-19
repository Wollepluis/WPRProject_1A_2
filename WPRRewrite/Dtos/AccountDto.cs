using WPRRewrite.Enums;

namespace WPRRewrite.Dtos;

public class AccountDto(AccountTypeEnum accountType, string email, string wachtwoord, string naam, int nummer, int adresId)
{
    public AccountTypeEnum AccountType { get; set; } = accountType;
    public string Email { get; set; } = email;
    public string Wachtwoord { get; set; } = wachtwoord;
    public string Naam { get; set; } = naam;
    public int Nummer { get; set; } = nummer;
    public int AdresId { get; set; } = adresId;
}