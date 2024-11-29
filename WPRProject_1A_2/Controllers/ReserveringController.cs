using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WPRProject_1A_2.Modellen.Accounts;
using WPRProject_1A_2.Modellen.Voertuigmodellen; // Namespace waarin jouw `Adres`-klasse zit


namespace WPRProject_1A_2.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class ReserveringController : ControllerBase
    {
        private CarAndAllContext _context;

        public ReserveringController()
        {
            _context = new CarAndAllContext();
        }

        [HttpGet("Krijg reservering")]
        public async Task<ActionResult<IEnumerable<Reservering>>> GetReserveringen()
        {
            return await _context.Reserveringen.ToListAsync();
        }
        
        [HttpGet("Krijg reservering/{id}")]
        public async Task<ActionResult<IEnumerable<Reservering>>>  GetReserveringenPerAccount(int id)
        {
            var reserveringen = await _context.Reserveringen.Select(r => r.AccountId == id).ToListAsync();
            if (reserveringen.Count == 0) return NotFound();
            return Ok(reserveringen);
        }
        
        [HttpPost("Maak reservering")]
        public async Task<IActionResult> PostReservering(DateTime begindatum, DateTime einddatum, string aardVanReis, string versteBestemming, int verwachteHoeveelheidKm, int accountId, long rijbewijsDocumentnummer, double totaalprijs)
        {
            Account account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return BadRequest();
            
            Reservering reservering = new Reservering(begindatum, einddatum, aardVanReis, versteBestemming, verwachteHoeveelheidKm, accountId, account, rijbewijsDocumentnummer, totaalprijs);
            reservering.CheckRijbewijs(rijbewijsDocumentnummer);
            _context.Reserveringen.Add(reservering);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
    }
}