using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public class Camper : Voertuig
{
    public Camper(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string voertuigstatus) 
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = voertuigstatus;
        
        Reserveringen = new List<Reservering>();
    }
    public Camper() { }
    
    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}