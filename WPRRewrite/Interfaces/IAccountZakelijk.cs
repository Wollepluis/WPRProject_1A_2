using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Modellen;

namespace WPRRewrite.Interfaces;

public interface IAccountZakelijk
{
    int BedrijfId { get; set; }

    [ForeignKey("BedrijfId")] Bedrijf Bedrijf { get; set; }
}