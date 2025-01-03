using WPRRewrite.Modellen;

namespace WPRRewrite.Dtos;

public class BedrijfstatistiekenDto
{
    public BedrijfstatistiekenDto(double kosten, int gehuurdeAutos, int hoeveelheidMedewerkers, string typeAbonnement, string bedrijfsnaam, Adres adres)
    {
        Kosten = kosten;
        GehuurdeAutos = gehuurdeAutos;
        HoeveelheidMedewerkers = hoeveelheidMedewerkers;
        TypeAbonnement = typeAbonnement;
        Bedrijfsnaam = bedrijfsnaam;
        Adres = adres;
    }

    public double Kosten { get; set; }
    public int GehuurdeAutos { get; set; }
    public int HoeveelheidMedewerkers { get; set; }
    public string TypeAbonnement { get; set; }
    public string Bedrijfsnaam { get; set; }
    public Adres Adres { get; set; }
    
    
}