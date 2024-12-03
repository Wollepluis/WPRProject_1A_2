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

        [HttpGet("Krijg alle autos")]
        public async Task<ActionResult<IEnumerable<Voertuig>>> GetAlleAutos()
        {
            //var autos = await _context.Voertuigen.Select(a => a.Voert)
            return Ok();
        }

        // [HttpGet("Filter")]
        // public async Task<ActionResult<IEnumerable<Voertuig>>> FilterVoertuig(VoertuigType voertuigType)
        // {
        //     try
        //     {
        //         return await _context.Voertuigen.Select(v => v.GetType());
        //     }
        //     catch (ArgumentException e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }



        [HttpPost("Voeg voertuig toe")]
        public async Task<IActionResult> PostAbonnement(Voertuig voertuig)
        {
            try
            {
                _context.Voertuigen.Add(voertuig);
                await _context.SaveChangesAsync();
                
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Verwijder voertuig")]
        public async Task<IActionResult> VerwijderVoertuig(int id)
        {
            Voertuig? voertuig = await _context.Voertuigen.FindAsync(id);
            if (voertuig == null)
            {
                return NotFound();
            }

            _context.Voertuigen.Remove(voertuig);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}