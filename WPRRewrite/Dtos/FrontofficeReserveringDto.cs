using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Dtos;

public class FrontofficeReserveringDto
{
    public int ReserveringsId { get; set; }
    public string Kenteken { get; set; }
    public string Merk { get; set; }
    public string Model { get; set; }
    public string Kleur { get; set; }
    public int Aanschafjaar { get; set; }
    public string VoertuigType { get; set; }
    public string BrandstofType { get; set; }
    
    public DateTime Begindatum { get; set; }
    public DateTime Einddatum { get; set; }
    public double TotaalPrijs { get; set; }
    public bool IsBetaald { get; set; }
    public bool IsGoedgekeurd { get; set; }
    public string Email { get; set; }

    public FrontofficeReserveringDto(int ReserveringsId, string Kenteken, string Merk, string Model, string Kleur, int Aanschafjaar,
        string VoertuigType, string BrandstofType, DateTime Begindatum, DateTime Einddatum, double TotaalPrijs,
        bool IsBetaald, bool IsGoedgekeurd, string Email)
    {
        this.ReserveringsId = ReserveringsId;
        this.Kenteken = Kenteken;
        this.Merk = Merk;
        this.Model = Model;
        this.Kleur = Kleur;
        this.Aanschafjaar = Aanschafjaar;
        this.VoertuigType = VoertuigType;
        this.BrandstofType = BrandstofType;
        this.Begindatum = Begindatum;
        this.Einddatum = Einddatum;
        this.TotaalPrijs = TotaalPrijs;
        this.IsBetaald = IsBetaald;
        this.IsGoedgekeurd = IsGoedgekeurd;
        this.Email = Email;
    }
}