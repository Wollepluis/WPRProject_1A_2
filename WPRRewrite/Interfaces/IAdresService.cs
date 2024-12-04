using WPRRewrite.Modellen;

namespace WPRRewrite.Interfaces;

public interface IAdresService
{
    Task<Adres?> ZoekAdresAsync(string postcode, int huisnummer);
}
