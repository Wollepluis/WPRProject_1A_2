using Newtonsoft.Json.Linq;
using WPRRewrite.Modellen;

namespace WPRRewrite.SysteemFuncties;

public class AdresService
{
    public static async Task<Adres?> ZoekAdresAsync(string postcode, int huisnummer)
    {
        if (string.IsNullOrWhiteSpace(postcode) || huisnummer <= 0)
        {
            throw new ArgumentException("Postcode and huisnummer must be valid.");
        }

        var apiUrl =
            $"https://api.pdok.nl/bzk/locatieserver/search/v3_1/free?q=postcode:{postcode} AND huisnummer:{huisnummer}";

        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                var adresData = json["response"]?["docs"]?[0];
                if (adresData != null)
                {
                    return new Adres()
                    {
                        Straatnaam = (string?)adresData["straatnaam"],
                        Huisnummer = (int?)adresData["huisnummer"] ?? 0,
                        Postcode = (string?)adresData["postcode"],
                        Woonplaats = (string?)adresData["woonplaatsnaam"],
                        Gemeente = (string?)adresData["gemeentenaam"],
                        Provincie = (string?)adresData["provincienaam"]
                    };
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error fetching address: {ex.Message}", ex);
        }

        return null;
    }
}