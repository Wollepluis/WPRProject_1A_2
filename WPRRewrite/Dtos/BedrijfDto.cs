namespace WPRRewrite.Dtos;

public class BedrijfDto(int kvkNummer, string bedrijfsnaam, string domeinnaam, string postcode, int huisnummer)
{
    public int KvkNummer { get; set; } = kvkNummer;
    public string Bedrijfsnaam { get; set; } = bedrijfsnaam;
    public string Domeinnaam { get; set; } = domeinnaam;
    public string Postcode { get; set; } = postcode;
    public int Huisnummer { get; set; } = huisnummer;
}