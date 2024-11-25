using WPRProject_1A_2.Modellen.Enums;

namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public class Auto : Voertuig
{
    public Auto(string kenteken, string merk, string model, string kleur, int aanschafjaar, double prijs) : base(kenteken, merk, model, kleur, aanschafjaar, prijs)
    {
    }
}