namespace WPRRewrite.Modellen.Voertuigen;

public class Camper : Voertuig
{
    public Camper(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs) : base(kenteken, merk, model, kleur, aanschafjaar, prijs)
    {
    }

    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}