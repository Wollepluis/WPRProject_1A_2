namespace WPRRewrite.Dtos;

public class BedrijfDto
{
    public int Kvknummer { get; set; }
    public string Bedrijfsnaam { get; set; }
    public string Postcode { get; set; }
    public int Huisnummer { get; set; }
    public int MaxMedewerkers { get; set; }
    public int MaxVoertuigen { get; set; }
    
    public BedrijfDto() { }
    public BedrijfDto(int kvknummer, string bedrijfsnaam, string postcode, int huisnummer, int maxMedewerkers, int maxVoertuigen)
    {
        Kvknummer = kvknummer;
        Bedrijfsnaam = bedrijfsnaam;
        Postcode = postcode;
        Huisnummer = huisnummer;
        MaxMedewerkers = maxMedewerkers;
        MaxVoertuigen = maxVoertuigen;
    }
}