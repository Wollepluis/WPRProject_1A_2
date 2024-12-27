using WPRRewrite.Enums;

namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string voertuigstatus, BrandstofType brandstofType) 
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        VoertuigStatus = voertuigstatus;
        this.BrandstofType = brandstofType;
        
        Reserveringen = new List<Reservering>();
    }
    public Caravan() { }

    
    public override List<Reservering> GetReserveringen()
    {
        return Reserveringen;
    }
}