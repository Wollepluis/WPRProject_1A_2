
namespace WPRRewrite.Dtos;

public class VoertuigDto
(
    string kenteken, 
    string merk, 
    string model, 
    string kleur, 
    int aanschafjaar, 
    int prijs,
    string voertuigStatus, 
    string voertuigType, 
    string brandstofType, 
    string aantalZitplaatsen
    )
{
    public string Kenteken { get; set; } = kenteken;
    public string Merk { get; set; } = merk;
    public string Model { get; set; } = model;
    public string Kleur { get; set; } = kleur;
    public int Aanschafjaar { get; set; } = aanschafjaar;
    public int Prijs { get; set; } = prijs;
    public string VoertuigStatus { get; set; } = voertuigStatus;
    public string VoertuigType { get; set; } = voertuigType;
    public string BrandstofType { get; set; } = brandstofType;
    public string AantalZitplaatsen { get; set; } = aantalZitplaatsen;
}