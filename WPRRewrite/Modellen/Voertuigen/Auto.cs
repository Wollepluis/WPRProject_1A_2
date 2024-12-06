using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public class Auto : Voertuig
{
    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}