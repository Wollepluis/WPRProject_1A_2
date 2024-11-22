namespace WPRProject_1A_2.Betaling;

public class Factuur
{
    int FactuurId { get; set; }
    int Prijs { get; set; }
    int Korting { get; set; }

    public Factuur(int factuurId, int prijs, int korting)
    {
        FactuurId = factuurId;
        Prijs = prijs;
        Korting = korting;
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
               $"Prijs: €{Prijs}\n" +
               $"Korting: {Korting}%\n" +
               $"Totaalbedrag na korting: €{StelFactuurOp()}";
    }
}



