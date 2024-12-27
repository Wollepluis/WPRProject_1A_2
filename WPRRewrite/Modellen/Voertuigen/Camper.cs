
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public class Camper : Voertuig
{
    public Camper(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string voertuigstatus, string brandstofType) 
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = voertuigstatus;
        BrandstofType = brandstofType;
        
        Reserveringen = new List<Reservering>();
    }
    public Camper() { }
    
    
}