namespace WPRRewrite.Dtos;

public class VoertuigReservering
{
    public DateTime Begindatum { get; set; }
    public DateTime Einddatum { get; set; }
    public int VoertuigId { get; set; }
    public int AccountId { get; set; }
    public string VoertuigStatus { get; set; }
}