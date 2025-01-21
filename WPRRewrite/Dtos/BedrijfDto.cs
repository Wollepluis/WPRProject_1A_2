namespace WPRRewrite.Dtos;

public class BedrijfDto
{
    public int KvkNummer { get; set; }
    public string Bedrijfsnaam { get; set; }
    public string Domeinnaam { get; set; }
    public string Postcode { get; set; }
    public int Huisnummer { get; set; }
    
    public BedrijfDto() { }
    public BedrijfDto(int kvkNummer, string bedrijfsnaam, string domeinnaam, string postcode, int huisnummer)
    {
        KvkNummer = kvkNummer;
        Bedrijfsnaam = bedrijfsnaam;
        Domeinnaam = domeinnaam;
        Postcode = postcode;
        Huisnummer = huisnummer;
        
    }
}