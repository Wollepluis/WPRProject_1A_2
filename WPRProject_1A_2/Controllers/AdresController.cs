using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WPRProject_1A_2.Modellen.Abonnementen; // Namespace waarin jouw `Adres`-klasse zit


namespace WPRProject_1A_2.Controllers
{

    
        [ApiController]
        [Route("api/[controller]")]
        public class AdresController : ControllerBase
        {
            private CarAndAllContext _context;
            public AdresController()
            {
                _context = new CarAndAllContext();
            }
            
            
            [HttpGet("zoek-adres")]
            public async Task<IActionResult> ZoekAdres(string postcode, int huisnummer)
            {
                if (string.IsNullOrWhiteSpace(postcode) || huisnummer <= 0)
                {
                    return BadRequest("Postcode en huisnummer moeten geldig zijn.");
                }

                string apiUrl =
                    $"https://api.pdok.nl/bzk/locatieserver/search/v3_1/free?q=postcode:{postcode} AND huisnummer:{huisnummer.ToString()}";

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        // HTTP GET-aanroep
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            // JSON-response ophalen
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            JObject json = JObject.Parse(jsonResponse);

                            // Verwerk de JSON-response
                            var adresData = json["response"]?["docs"]?[0]; // Eerste resultaat
                            if (adresData != null)
                            {
                                // Map de JSON-response naar de Adres-klasse
                                Adres adress = new Adres()
                                {
                                    Straatnaam = (string?)adresData["straatnaam"],
                                    Huisnummer = (int)adresData["huisnummer"],
                                    Postcode = (string?)adresData["postcode"],
                                    Woonplaats = (string?)adresData["woonplaatsnaam"],
                                    Gemeente = (string?)adresData["gemeentenaam"],
                                    Provincie = (string?)adresData["provincienaam"]
                                };
                                
                                return Ok(adress);
                            }
                            else
                            {
                                return NotFound("Geen adres gevonden voor de opgegeven postcode en huisnummer.");
                            }
                        }

                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Er is een fout opgetreden: {ex.Message}");
                }
            }
            
            [HttpPost("Sla adres op")]
            public async Task<IActionResult> PostAdres(Adres adres)
            {
                _context.Adressen.Add(adres);

                // Sla de wijzigingen op in de database
                await _context.SaveChangesAsync();
                return Ok(adres);
            }
        }
    }

