using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/Reservering")]
public class ReserveringController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public ReserveringController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


    [HttpGet("KrijgAlleReserveringen")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringen()
    {
        return await _context.Reserveringen.ToListAsync();
    }

    [HttpGet("KrijgAlleReserveringenPerAccount")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringenPerAccount(int accountId)
    {
        var reserveringen = await _context.Reserveringen.Where(r => r.Account.AccountId == accountId).ToListAsync();
        if (!reserveringen.Any())
        {
            return NotFound($"Geen reserveringen gevonden voor accountId: {accountId}");
        }
        return Ok(reserveringen);
    }

    [HttpGet("KrijgAlleReserveringenPerVoertuig")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringenPerVoertuig(int voertuigId)
    {
        var voertuig = await _context.Voertuigen.FindAsync(voertuigId);
        if (voertuig == null) return NotFound();
        voertuig.GetReserveringen();
        return Ok(voertuig);
    }

    [HttpPost("PostReservering")]
    public async Task<ActionResult<IEnumerable<Reservering>>> PostReservering(ReserveringDto reserveringDto)
    {
        if (reserveringDto == null) return BadRequest();

        Reservering reservering = new Reservering(reserveringDto.Begindatum, reserveringDto.Einddatum, reserveringDto.TotaalPrijs, reserveringDto.VoertuigId, reserveringDto.AccountId);
        
        _context.Reserveringen.Add(reservering);
        
        await _context.SaveChangesAsync();
        
        return Ok(reservering);
    }
    
    [HttpDelete("VerwijderReservering")]
    public async Task<IActionResult> DeleteReservering(int reserveringId)
    {
        var reservering = await _context.Reserveringen.FindAsync(reserveringId);
        if (reservering == null)
        {
            return NotFound();
        }

        _context.Reserveringen.Remove(reservering);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}