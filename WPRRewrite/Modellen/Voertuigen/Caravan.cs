namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs) : base(kenteken, merk, model, kleur, aanschafjaar, prijs)
    {
    }

    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}