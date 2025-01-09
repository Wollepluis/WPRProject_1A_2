namespace WPRRewrite.Dtos;

public class BedrijfDto
{
    public int Kvknummer { get; set; }
    public string Bedrijfsnaam { get; set; }
    public string Domeinnaam { get; set; }
    public string Postcode { get; set; }
    public int Huisnummer { get; set; }
    
    public BedrijfDto() { }
    public BedrijfDto(int kvknummer, string bedrijfsnaam, string domeinnaam, string postcode, int huisnummer)
    {
        Kvknummer = kvknummer;
        Bedrijfsnaam = bedrijfsnaam;
        Domeinnaam = domeinnaam;
        Postcode = postcode;
        Huisnummer = huisnummer;
        
    }
}