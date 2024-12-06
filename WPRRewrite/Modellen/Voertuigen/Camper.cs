namespace WPRRewrite.Modellen.Voertuigen;

public class Camper : Voertuig
{
    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}