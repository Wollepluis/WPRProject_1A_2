using WPRProject_1A_2.Modellen.Abonnementen;

namespace WPRProject_1A_2.Modellen.Betalingen;

public class Factuur 
{
    public int FactuurId { get; set; }
    public int Prijs { get; set; }
    public int Korting { get; set; }
    public Bedrijf Bedrijf { get; set; }

    public Factuur(int factuurId, int prijs, int korting, Bedrijf bedrijf)
    {
        FactuurId = factuurId;
        Prijs = prijs;
        Korting = korting;
        Bedrijf = bedrijf;
    }

    public int StelFactuurOp()
    {
        int kortingsBedrag = (Prijs * Korting) / 100;
        int totaalPrijs = Prijs - kortingsBedrag;
        return totaalPrijs;
    }

    public override string ToString()
    {
        return $"Factuur ID: {FactuurId}\n" +
               $"Bedrijfsnaam: {Bedrijf.Bedrijfsnaam}\n" +
               $"Domeinnaam: {Bedrijf.Domeinnaam}\n" +
               $"Adres: {Bedrijf.Adres}\n" +
               $"Kvk-nummer: {Bedrijf.KvkNummer}\n" +
               $"Abonnement: {Bedrijf.Abonnement}\n" +
               $"Prijs: €{Prijs}\n" +
               $"Korting: {Korting}%\n" +
               $"Totaalbedrag na korting: €{StelFactuurOp()}";
    }
}



