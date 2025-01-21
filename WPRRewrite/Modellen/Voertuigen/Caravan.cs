namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public Caravan() { }
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, 
        string voertuigStatus, string voertuigType, string brandstofType, int aantalZitplaatsen) 
        : base(kenteken, merk, model, kleur, aanschafjaar, prijs, voertuigStatus, voertuigType, brandstofType, 
            aantalZitplaatsen) { }
}