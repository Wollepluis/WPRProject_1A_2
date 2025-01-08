using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Modellen;

public class Schadeclaim
{
    public int SchadeclaimId { get; set; }
    public string Beschrijving { get; set; }
    public DateTime Datum { get; set; }
    public Reparatie Reparatie { get; set; }
}