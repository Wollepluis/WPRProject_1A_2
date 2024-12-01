namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public class Caravan : Voertuig
{
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, double prijs) : 
        base(kenteken, merk, model, kleur, aanschafjaar, prijs)
    {
    }
}