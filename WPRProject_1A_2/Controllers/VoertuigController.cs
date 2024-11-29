using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WPRProject_1A_2.Modellen.Abonnementen;
using WPRProject_1A_2.Modellen.Accounts;
using WPRProject_1A_2.Modellen.Enums;
using WPRProject_1A_2.Modellen.Voertuigmodellen; // Namespace waarin jouw `account`-klasse zit


namespace WPRProject_1A_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoertuigController : ControllerBase
    {
        private CarAndAllContext _context;

        public VoertuigController()
        {
            _context = new CarAndAllContext();
        }

        // [HttpGet("Filter")]
        // public async Task<ActionResult<IEnumerable<Voertuig>>> FilterVoertuig()
        // {
        //     
        // }

        [HttpPost("Voeg voertuig toe")]
        public async Task<IActionResult> PostAbonnement(VoertuigType voertuigType, string kenteken, string merk , string model, string kleur, int aanschafjaar, double prijs)
        {
            try
            {
                Voertuig voertuig =
                    VoertuigFactory.CreateVoertuig(voertuigType, kenteken, merk, model, kleur, aanschafjaar, prijs);
                _context.Voertuigen.Add(voertuig);
                await _context.SaveChangesAsync();
                return Ok($"{voertuigType}: '{merk} {model}' met kenteken {kenteken} is toegevoegd aan de database");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}