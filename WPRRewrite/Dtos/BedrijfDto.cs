namespace WPRRewrite.Dtos;

public class BedrijfDto
{
    public int Kvknummer { get; set; }
    public string Bedrijfsnaam { get; set; }
    public int BedrijfAdres { get; set; }
    public string Domeinnaam { get; set; }
    public int AbonnementId { get; set; }
    
    public BedrijfDto() { }
    public BedrijfDto(int kvknummer, string bedrijfsnaam, int bedrijfAdres, string domiennaam)
    {
        Kvknummer = kvknummer;
        Bedrijfsnaam = bedrijfsnaam;
        BedrijfAdres = bedrijfAdres;
        Domeinnaam = domiennaam;
    }
}