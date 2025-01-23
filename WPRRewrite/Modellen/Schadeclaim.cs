using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Modellen;

public class Schadeclaim
{
    public Schadeclaim(string beschrijving, int voertuigId, DateTime datum)
    {
        Beschrijving = beschrijving;
        Datum = datum;
        VoertuigId = voertuigId;
        
    }

    public int SchadeclaimId { get; set; }
    public string Beschrijving { get; set; }
    public DateTime Datum { get; set; }
    public int? ReparatieId { get; set; }
    [ForeignKey(nameof(ReparatieId))]
    public Reparatie Reparatie { get; set; }
    public int VoertuigId { get; set; }
    [ForeignKey(nameof(VoertuigId))]
    public Voertuig Voertuig { get; set; }
}