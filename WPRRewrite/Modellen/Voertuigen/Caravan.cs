
namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string voertuigstatus, string brandstofType) 
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

    
    
}