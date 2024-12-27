namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, string voertuigstatus) 
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
    public Caravan() { }

    
    
}