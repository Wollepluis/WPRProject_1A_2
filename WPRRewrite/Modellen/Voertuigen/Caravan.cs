
using WPRRewrite.Enums;

namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public Caravan() { }
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, 
        VoertuigStatusEnum voertuigStatus) 
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = voertuigStatus;
    }
}