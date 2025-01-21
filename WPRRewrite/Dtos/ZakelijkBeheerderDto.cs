namespace WPRRewrite.Dtos;

public class ZakelijkBeheerderDto(string email, string wachtwoord, string accountType)
{
    public string Email { get; } = email;
    public string Wachtwoord { get; } = wachtwoord;
    public string AccountType { get; } = accountType;
}