using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPRProject_1A_2.Voertuigmodellen;

public class Schadeclaim
{
    [Key]
    public int SchadeclaimId { get; set; }
    
    public string? Beschrijving { get; set; }
    public required DateTime Datum { get; set; }
    //List<Foto> Fotos { get; set; }
    
    public int VoertuigId { get; set; }
    [ForeignKey("VoertuigId")]
    public required Voertuig Voertuig { get; set; }
    
    public int ReparatieId { set; get; }
    [ForeignKey("ReparatieId")]
    public required List<Reparatie> Reparaties
    {
        set { reparaties = value; }
        get { return reparaties; }
    }
    private List<Reparatie> reparaties;
    
    public enum StatusEnum {Behandeling, Afgehandeld}
    public required StatusEnum Status { get; set; }

    public void ReparatieToevoegen(Reparatie reparatie)
    {
        reparaties.Add(reparatie);
    }
    
}