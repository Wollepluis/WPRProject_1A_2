using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace WPRProject_1A_2.Modellen.Abonnementen;

public class Adres
{
    [Key]
    public int AdresId { get; set; }
    [MaxLength(50)]
    public required string Straatnaam { get; set; }

    [Range(1, 9999)]
    public required int Huisnummer { get; set; }

    [StringLength(6)]
    public required string Postcode { get; set; }

    [MaxLength(50)]
    public required string Woonplaats { get; set; }

    [MaxLength(50)]
    public required string Gemeente { get; set; }

    [MaxLength(50)]
    public required string Provincie { get; set; }

    public Adres()
    {
    }

}
