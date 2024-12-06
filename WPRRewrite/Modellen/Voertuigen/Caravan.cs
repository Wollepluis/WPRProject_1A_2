namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}