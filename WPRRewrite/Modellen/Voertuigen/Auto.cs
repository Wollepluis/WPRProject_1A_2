namespace WPRRewrite.Modellen.Voertuigen;

public class Auto : Voertuig
{
    public Auto() { }
    public Auto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, 
        string voertuigStatus, string voertuigType, string brandstofType, string aantalZitplaatsen) 
        : base(kenteken, merk, model, kleur, aanschafjaar, prijs, voertuigStatus, voertuigType, brandstofType, 
            aantalZitplaatsen) { }
}