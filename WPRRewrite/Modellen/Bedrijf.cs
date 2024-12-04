namespace WPRRewrite.Modellen;

public class Bedrijf
{
    public int BedrijfId { get; set; }
    public string Bedrijfsnaam { get; set; }
    public int BedrijfAdres { get; set; }
    public string Domeinnaam { get; set; }
    public int KvkNummer { get; set; }
    
    
    public void UpdateBedrijf(Bedrijf updatedBedrijf)
    {
        Bedrijfsnaam = updatedBedrijf.Bedrijfsnaam;
        BedrijfAdres = updatedBedrijf.BedrijfAdres;
        Domeinnaam = updatedBedrijf.Domeinnaam;
        KvkNummer = updatedBedrijf.KvkNummer;
    }
}