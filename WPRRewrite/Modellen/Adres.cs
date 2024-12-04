namespace WPRRewrite.Modellen;

public class Adres
{
    public int AdresId { get; set; }
    public string Straatnaam { get; set; }
    public int Huisnummer { get; set; }
    public string Postcode { get; set; }
    public string Woonplaats { get; set; }
    public string Gemeente { get; set; }
    public string Provincie { get; set; }
    
    public void UpdateAdres(Adres updatedAdres)
    {
        Straatnaam = updatedAdres.Straatnaam;
        Huisnummer = updatedAdres.Huisnummer;
        Postcode = updatedAdres.Postcode;
        Woonplaats = updatedAdres.Woonplaats;
        Gemeente = updatedAdres.Gemeente;
        Provincie = updatedAdres.Provincie;
    }
}