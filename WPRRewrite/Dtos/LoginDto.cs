namespace WPRRewrite.Dtos;

public class LoginDto
{
    public string Email { get; set; }
    public string Wachtwoord { get; set; }

    public LoginDto(string email, string wachtwoord)
    {
        Email = email;
        Wachtwoord = wachtwoord;
    }
}