using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("Krijg Alle Voertuigen")]
        public async Task<ActionResult<IEnumerable<Voertuig>>> GetAlleVoertuigen()
        {
            return await _context.Voertuigen.ToListAsync();
        }
        
        // [HttpGet("Filter")]
        // public async Task<ActionResult<IEnumerable<Voertuig>>> FilterVoertuig(VoertuigType voertuigType)
        // {
        //     try
        //     {
        //         // Filter the vehicles based on the provided voertuigType
        //         var filteredVoertuigen = await _context.Voertuigen
        //             .Where(v => v.GetType().Name == voertuigType.ToString()) // Adjust based on the actual type name in the model
        //             .ToListAsync();
        //
        //         // If no vehicles found, return a 404 Not Found response
        //         if (filteredVoertuigen == null || !filteredVoertuigen.Any())
        //         {
        //             return NotFound($"No vehicles found for the type {voertuigType}");
        //         }
        //
        //         return Ok(filteredVoertuigen);
        //     }
        //     catch (ArgumentException e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }



        [HttpPost("Voeg voertuig toe")]
        public async Task<IActionResult> PostAbonnement(VoertuigType voertuigType, string kenteken, string merk , 
            string model, string kleur, int aanschafjaar, double prijs)
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