using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRProject_1A_2.Modellen.Enums;

namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public class Schadeclaim
{
    [Key]
    public int SchadeclaimId { get; set; }
    public string? Beschrijving { get; set; }
    [DataType(DataType.DateTime)]
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
    public SchadeclaimStatus SchadeclaimStatus { get; set; }

    public Schadeclaim(string beschrijving, DateTime datum)
    {
        Beschrijving = beschrijving;
        Datum = datum;
        SchadeclaimStatus = SchadeclaimStatus.InBehandeling;

        Reparaties = new List<Reparatie>();
    }
    
    public void ReparatieToevoegen(Reparatie reparatie)
    {
        reparaties.Add(reparatie);
    }
    
}