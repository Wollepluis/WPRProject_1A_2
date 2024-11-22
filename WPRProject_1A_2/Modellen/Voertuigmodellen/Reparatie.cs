using System.ComponentModel.DataAnnotations;

namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public class Reparatie
{
    [Key]
    public int ReparatieId { set; get; }
    public string? Beschrijving { set; get; }
    public required DateTime Reparatiedatum { set; get; }
    public required List<string> Opmerkingen { set; get; }
}