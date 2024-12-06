using WPRRewrite.Enums;
using WPRRewrite.Interfaces;

namespace WPRRewrite.Modellen.Voertuigen;

public abstract class Voertuig : IVoertuig
{
    public int VoertuigId { get; set; }
    public string Kenteken { get; set; }
    public string Merk { get; set; }
    public string Model { get; set; }
    public string Kleur { get; set; }
    public int Aanschafjaar { get; set; }
    public int Prijs { get; set; }

    public List<Reservering> Reserveringen { get; set; }
    public abstract List<Reservering> GetReserveringen();
    public void UpdateVoertuig(IVoertuig updatedVoertuig)
    {
        Kenteken = updatedVoertuig.Kenteken;
        Merk = updatedVoertuig.Merk;
        Model = updatedVoertuig.Model;
        Kleur = updatedVoertuig.Kleur;
        Aanschafjaar = updatedVoertuig.Aanschafjaar;
        Prijs = updatedVoertuig.Prijs;
    }

    public Voertuig(string kenteken, string merk, string model, string kleur, int aanschafjaar, int prijs)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        Reserveringen = new List<Reservering>();
    }
}