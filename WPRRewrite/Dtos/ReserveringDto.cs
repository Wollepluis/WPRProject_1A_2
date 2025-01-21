namespace WPRRewrite.Dtos;

public class ReserveringDto(DateOnly begindatum, DateOnly einddatum, int totaalPrijs, int voertuigId, int accountId,
    string voertuigStatus)
{
    public DateOnly Begindatum { get; set; } = begindatum;
    public DateOnly Einddatum { get; set; } = einddatum;
    public int TotaalPrijs { get; set; } = totaalPrijs;
    public int VoertuigId { get; set; } = voertuigId;
    public int AccountId { get; set; } = accountId;
    public string VoertuigStatus { get; set; } = voertuigStatus;
}