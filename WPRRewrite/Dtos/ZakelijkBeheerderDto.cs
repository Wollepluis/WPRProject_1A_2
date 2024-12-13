namespace WPRRewrite.Dtos;

public class ZakelijkBeheerderDto : AccountDto
{
    public ZakelijkBeheerderDto(string email, string wachtwoord)
    {
        Email = email;
        Wachtwoord = wachtwoord;
    }
}