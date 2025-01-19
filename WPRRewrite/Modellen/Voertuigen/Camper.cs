
using WPRRewrite.Enums;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public class Camper : Voertuig
{
    public Camper() { }
    public Camper(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, 
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