using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Modellen;

public class Reservering
{
    public int ReserveringId { get; set; }
    public DateTime Begindatum { get; set; }
    public DateTime Einddatum { get; set; }
    public string AardVanReis { get; set; }
    public string VersteBestemming { get; set; }
    public int VerwachteHoeveelheidkm { get; set; }
    public int Rijbewijsnummer { get; set; }
    public Double TotaalPrijs { get; set; }
    public bool IsBetaald { get; set; }
    public List<Voertuig> GereserveerdeVoertuigen { get; set; }
    public Account Account { get; set; }
}