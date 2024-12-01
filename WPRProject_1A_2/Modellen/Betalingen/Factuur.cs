using System.ComponentModel.DataAnnotations;
using WPRProject_1A_2.Modellen.Abonnementen;
using WPRProject_1A_2.Modellen.Voertuigmodellen;

namespace WPRProject_1A_2.Modellen.Betalingen;

public class Factuur 
{
    [Key]
    public int FactuurId { get; set; }
    public Reservering Reservering { get; set; }
    [DataType(DataType.Currency)]
    public double Prijs { get; set; }
    public Bedrijf Bedrijf { get; set; }

    public Factuur() {}

    public Factuur(double prijs, Bedrijf bedrijf)
    {
        Prijs = prijs;
        Bedrijf = bedrijf;
    }
    
    private double StelFactuurOp()
    {
        double totaalPrijs = 0;
        foreach (var v in Reservering.BesteldeVoertuigen)
        {
            double prijs = v.Prijs;
            if (Bedrijf.Abonnement is PayAsYouGo)
            {
                double prijs2 = BerekenKorting(prijs, v);
                totaalPrijs += prijs2;
            }
            else
            {
                Console.WriteLine($"Artikel: {Prijs}");
                totaalPrijs += prijs;
            }
        }
        return totaalPrijs;
    }
    
    private double BerekenKorting(double prijs, Voertuig v)
    {
        double nieuwePrijs = prijs;
        int korting = 0;
        switch (v) 
        { 
            case Caravan:
                nieuwePrijs *= 0.60;
                korting = 40;
                break;
            case Camper:
                nieuwePrijs *= 0.70;
                korting = 30;
                break;
            case Auto:
                nieuwePrijs *= 0.50;
                korting = 50;
                break;
        }
        Console.WriteLine($"Artikel: {nieuwePrijs} + {korting}");
        return nieuwePrijs;
    }

    public override string ToString()
    {
        return $"Factuur ID: {FactuurId}\n" +
               $"Bedrijfsnaam: {Bedrijf.Bedrijfsnaam}\n" +
               $"Domeinnaam: {Bedrijf.Domeinnaam}\n" +
               $"Adres: {Bedrijf.Adres}\n" +
               $"Kvk-nummer: {Bedrijf.KvkNummer}\n" +
               $"Abonnement: {Bedrijf.Abonnement}\n" +
               
               StelFactuurOp();
    }
}



