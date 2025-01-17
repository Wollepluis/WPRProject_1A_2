namespace WPRRewrite.Dtos;

public class LoginDto(string email, string wachtwoord)
{
    public string Email { get; } = email;
    public string Wachtwoord { get; } = wachtwoord;
}