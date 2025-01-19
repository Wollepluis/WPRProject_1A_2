using WPRRewrite.Enums;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public class Auto : Voertuig
{
    public Auto() { }
    public Auto(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, 
        VoertuigStatusEnum voertuigStatus, VoertuigTypeEnum voertuigType) 
        : base(kenteken, merk, model, kleur, aanschafjaar, prijs, voertuigStatus, voertuigType) { }
}