
using WPRRewrite.Enums;

namespace WPRRewrite.Modellen.Voertuigen;

public class Caravan : Voertuig
{
    public Caravan() { }
    public Caravan(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs, 
        VoertuigStatusEnum voertuigStatus, VoertuigTypeEnum voertuigType) 
        : base(kenteken, merk, model, kleur, aanschafjaar, prijs, voertuigStatus, voertuigType) { }
}