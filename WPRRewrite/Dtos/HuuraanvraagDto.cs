using System.ComponentModel.DataAnnotations;

namespace WPRRewrite.Dtos;

public class HuuraanvraagDto
{
    public int ReserveringId { get; set; }
    [MaxLength(255)] public string? Comment { get; set; }
    public bool Keuze { get; set; }
}