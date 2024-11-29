using WPRProject_1A_2.Modellen.Enums;

namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public static class VoertuigFactory
{
    public static Voertuig CreateVoertuig(VoertuigType voertuigType, string kenteken, string merk, string model, 
        string kleur, int aanschafjaar, double prijs)
    {
        return voertuigType switch
        {
            VoertuigType.Auto => new Auto(kenteken, merk, model, kleur, aanschafjaar, prijs),
            VoertuigType.Camper => new Camper(kenteken, merk, model, kleur, aanschafjaar, prijs),
            VoertuigType.Caravan => new Caravan(kenteken, merk, model, kleur, aanschafjaar, prijs),
            _ => throw new ArgumentException($"Invalid voertuigtype {voertuigType}")
        };
    }
}


