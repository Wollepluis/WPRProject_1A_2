using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public class Auto : Voertuig
{
    public Auto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs) : base(kenteken, merk, model, kleur, aanschafjaar, prijs)
    {
    }

    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}