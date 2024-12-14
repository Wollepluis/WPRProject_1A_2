namespace WPRRewrite.Dtos;

public class ZakelijkBeheerderDto : AccountDto
{
    public int BedrijfId { get; set; }
    public ZakelijkBeheerderDto(string email, string wachtwoord, int bedrijfsId)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        BedrijfId = bedrijfsId;
    }
    public ZakelijkBeheerderDto(string email, string wachtwoord)
    {
        Email = email;
        Wachtwoord = wachtwoord;
    }

    public ZakelijkBeheerderDto()
    {
        
    }
}