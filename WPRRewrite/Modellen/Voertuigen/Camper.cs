
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public class Camper : Voertuig
{
    public Camper(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string voertuigstatus, int aantalzitplaatsen, string brandstofType) 
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = voertuigstatus;
        AantalZitPlaatsen = aantalzitplaatsen;
        BrandstofType = brandstofType;
        
        Reserveringen = new List<Reservering>();
    }
    public Camper() { }
    
    
}