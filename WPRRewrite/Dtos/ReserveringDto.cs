using WPRRewrite.Enums;

namespace WPRRewrite.Dtos;

public class ReserveringDto(DateOnly begindatum, DateOnly einddatum, int totaalPrijs, int voertuigId, int accountId,
    VoertuigStatusEnum voertuigStatus)
{
    public DateOnly Begindatum { get; set; } = begindatum;
    public DateOnly Einddatum { get; set; } = einddatum;
    public int TotaalPrijs { get; set; } = totaalPrijs;
    public int VoertuigId { get; set; } = voertuigId;
    public int AccountId { get; set; } = accountId;
    public VoertuigStatusEnum VoertuigStatus { get; set; } = voertuigStatus;
}