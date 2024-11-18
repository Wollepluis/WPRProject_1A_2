using System.ComponentModel.DataAnnotations;

namespace WPRProject_1A_2.Voertuigmodellen;

public class Voertuig
{
    [Key]
    public int Id { set; get; }


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
    private List<Reservering> reserveringen { get; set; }
    public List<Reservering> Reserveringen
    {
        set { reserveringen = value; }
        get { return reserveringen; }
    }
    
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
        Reserveringen.Add(reservering);
    }

    public void SchadeClaimToevoegen()
    {
        //Aanpassen
    }

    public List<Voertuig> VoertuigFilter()
    {
        List<Voertuig> voertuigen = new List<Voertuig>();
        //
        // Nog aanpassen!!!!!!!
        //
        return voertuigen;
    }


}