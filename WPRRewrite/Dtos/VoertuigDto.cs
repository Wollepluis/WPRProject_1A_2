using WPRRewrite.Enums;

namespace WPRRewrite.Dtos;

public class VoertuigDto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs,
    VoertuigStatusEnum voertuigStatus, VoertuigTypeEnum voertuigType)
{
    public string Kenteken { get; set; } = kenteken;
    public string Merk { get; set; } = merk;
    public string Model { get; set; } = model;
    public string Kleur { get; set; } = kleur;
    public int Aanschafjaar { get; set; } = aanschafjaar;
    public int Prijs { get; set; } = prijs;
    public VoertuigStatusEnum VoertuigStatus { get; set; } = voertuigStatus;
    public VoertuigTypeEnum VoertuigType { get; set; } = voertuigType;
}