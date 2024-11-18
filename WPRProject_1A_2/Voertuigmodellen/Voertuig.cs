using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPRProject_1A_2.Voertuigmodellen;

public class Voertuig
{
    [Key]
    public int VoertuigId { set; get; }


    public enum TypeVoertuigEnum { Auto, Camper, Caravan }
    public required TypeVoertuigEnum TypeVoertuig { set; get; }
    
    public enum VoertuigStatusEnum { Beschikbaar, Verhuurd, InReparatie, Geblokkeerd }
    public required VoertuigStatusEnum VoertuigStatus { set; get; }
    
    private string kenteken;
    public string Kenteken
    {
        set { kenteken = value; }
        get { return kenteken; }
    }
    
    
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
    
    public required string Merk { set; get; }
    public required string Model { set; get; }
    public required string Kleur { set; get; }
    public required int Aanschafjaar { set; get; }

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