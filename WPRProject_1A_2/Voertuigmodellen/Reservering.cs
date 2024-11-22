using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPRProject_1A_2.Voertuigmodellen;

public class Reservering
{
    [Key]
    public int ReserveringId { get; set; }
    
    public int VoertuigId { get; set; }
    [ForeignKey("VoertuigId")]
    public required Voertuig Voertuig { get; set; }
    
    public required DateTime Begindatum { get; set; }
    public required DateTime Einddatum { get; set; }

    public Reservering()
    {
        
    }
}