namespace WPRRewrite.Dtos;

public class CaravanDto : VoertuigDto
{
    public CaravanDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs)
    {
        
    }

    public CaravanDto(string caravanKenteken, string caravanMerk, string caravanModel, string caravanKleur, int caravanAanschafjaar, int caravanPrijs, string caravanVoertuigStatus)
    {
        throw new NotImplementedException();
    }
}