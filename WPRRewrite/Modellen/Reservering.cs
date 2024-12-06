using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;

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

    public bool IsBetaald
    {
        get { return isBetaald; }
        set
        {
            isBetaald = value;
        }
    }

    private bool isBetaald;
    public List<IVoertuig> GereserveerdeVoertuigen { get; set; }
    public int Account { get; set; }

    public void Update(Reservering reservering)
    {
        Begindatum = reservering.Begindatum;
        Einddatum = reservering.Einddatum;
        AardVanReis = reservering.AardVanReis;
        VersteBestemming = reservering.VersteBestemming;
        VerwachteHoeveelheidkm = reservering.VerwachteHoeveelheidkm;
        Rijbewijsnummer = reservering.Rijbewijsnummer;
        TotaalPrijs = BerekenPrijs();
    }

    private Double BerekenPrijs()
    {
        return 0.4; // NOG MAKEN
    }
}