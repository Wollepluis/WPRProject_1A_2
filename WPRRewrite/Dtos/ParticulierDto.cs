namespace WPRRewrite.Dtos;

public class ParticulierDto : AccountDto
{
    public string Naam { get; set; }
    public int AdresId { get; set; }
    public int Telefoonnummer { get; set; }

    public ParticulierDto(string email, string wachtwoord, string naam, int adresId, int telefoonnummer)
    {
        Email = email;
        Wachtwoord = wachtwoord;
        Naam = naam;
        AdresId = adresId;
        Telefoonnummer = telefoonnummer;
    }
}