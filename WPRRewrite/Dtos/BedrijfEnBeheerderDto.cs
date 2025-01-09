using WPRRewrite.Modellen.Abonnementen;

namespace WPRRewrite.Dtos;

public class BedrijfEnBeheerderDto
{
    public BedrijfDto Bedrijf { get; set; }
    public ZakelijkBeheerderDto Beheerder { get; set; }
    public Abonnement Abonnement { get; set; }
}
