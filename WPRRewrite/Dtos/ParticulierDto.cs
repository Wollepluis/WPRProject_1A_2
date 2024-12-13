namespace WPRRewrite.Dtos;

public class ParticulierDto : AccountDto
{
    public string Naam { get; set; }
    public int Telefoonnummer { get; set; }
    public string Postcode { get; set; }
    public int Huisnummer { get; set; }

    public ParticulierDto(string email, string wachtwoord, string naam, int telefoonnummer, string postcode, int huisnummer)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        Naam = naam;
        Telefoonnummer = telefoonnummer;
        Postcode = postcode;
        Huisnummer = huisnummer;
    }
}