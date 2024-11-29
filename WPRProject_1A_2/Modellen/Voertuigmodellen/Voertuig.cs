using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRProject_1A_2.Modellen.Enums;

namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public class Voertuig
{
    [Key]
    public int VoertuigId { set; get; }
    
    private string kenteken;
    public string Kenteken
    {
        set { kenteken = value; }
        get { return kenteken; }
    }
    
    public string Merk { set; get; }
    public string Model { set; get; }
    public string Kleur { set; get; }
    [Range(1885, 9999)]
    public int Aanschafjaar { set; get; }
    public VoertuigStatus Voertuigstatus { get; set; }
    
    public int ReserveringId { set; get; }
    [ForeignKey("ReserveringId")]
    public List<Reservering> Reserveringen
    {
        set { reserveringen = value; }
        get { return reserveringen; }
    }
    private List<Reservering> reserveringen { get; set; }
    
    public int SchadeclaimId { set; get; }
    [ForeignKey("SchadeclaimId")]
    public List<Schadeclaim> Schadeclaims
    {
        set { schadeclaims = value; }
        get { return schadeclaims; }
    }
    private List<Schadeclaim> schadeclaims { get; set; }

    public double Prijs;

    public Voertuig(string kenteken, string merk, string model, string kleur, int aanschafjaar, double prijs)
    {
        Kenteken = kenteken;
        Merk = merk;
        Model = model;
        Kleur = kleur;
        Aanschafjaar = aanschafjaar;
        Prijs = prijs;
        Voertuigstatus = VoertuigStatus.Beschikbaar;

        Reserveringen = new List<Reservering>();
        Schadeclaims = new List<Schadeclaim>();

    }

    public void SetIsVerhuurd()
    {
        
    }

    public bool GetIsVerhuurd()
    {
        return false; //Nog aanpassen!!!!!!!!!!!!!!
    }

    public void ReserveringToevoegen(Reservering reservering)
    {
        reserveringen.Add(reservering);
    }

    public void SchadeClaimToevoegen(Schadeclaim schadeclaim)
    {
        schadeclaims.Add(schadeclaim);
    }


}