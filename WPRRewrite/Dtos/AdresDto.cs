namespace WPRRewrite.Dtos;

public class AdresDto(string postcode, int huisnummer)
{
    public string Postcode { get; set; } = postcode;
    public int Huisnummer { get; set; } = huisnummer;
}