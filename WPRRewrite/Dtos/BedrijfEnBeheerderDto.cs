using WPRRewrite.Modellen.Abonnementen;

namespace WPRRewrite.Dtos;

public class BedrijfEnBeheerderDto
{
    public BedrijfDto Bedrijf { get; set; }
    public ZakelijkBeheerderDto Account { get; set; }
    public AbonnementDto Abonnement { get; set; }
}
