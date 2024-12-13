using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Dtos;

public class ZakelijkHuurderDto : AccountDto
{
    public int BedrijfId { get; set; }

    public ZakelijkHuurderDto()
    {
        
    }
    public ZakelijkHuurderDto(string email, string wachtwoord, int bedrijfsId)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        BedrijfId = bedrijfsId;
    }
}