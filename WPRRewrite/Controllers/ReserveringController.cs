using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Enums;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen.Voertuigen;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("[Controller]")]
public class ReserveringController(Context context) : ControllerBase
{
    private readonly Context _context = context ?? throw new ArgumentNullException(nameof(context));

    [HttpGet("GetReserveringen")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetReserveringen([FromQuery] int? reserveringId, 
        [FromQuery] int? accountId, [FromQuery] int? voertuigId, [FromQuery] int? bedrijfId)
    {
        try
        {
            IQueryable<Reservering> query = _context.Reserveringen;

            if (reserveringId.HasValue)
            {
                var reservering = await query
                    .FirstOrDefaultAsync(r => r.ReserveringId == reserveringId);
                if (reservering == null) 
                    return NotFound(new { Message = $"Reservering met ID {reserveringId} niet gevonden" });
                
                return Ok(reservering);
            }
            
            if (voertuigId.HasValue)
            {
                var reservering = await query
                    .FirstOrDefaultAsync(r => r.VoertuigId == voertuigId);
                if (reservering == null)
                    return NotFound(new { Message = $"Voertuig heeft geen reservering" });
                
                return Ok(reservering);
            }
            
            if (accountId.HasValue)
            {
                query = query.Where(r => r.AccountId == accountId);
            }

            if (bedrijfId.HasValue)
            {
                query = query.Where(r => r.Account.AccountType == AccountTypeEnum.Zakelijk &&
                                 ((AccountZakelijk)r.Account).BedrijfId == bedrijfId);
            }
            
            var projectedQuery = query.Select(r => new
            {
                r.ReserveringId,
                r.Begindatum,
                r.Einddatum,
                r.TotaalPrijs,
                r.IsBetaald,
                r.IsGoedgekeurd,
                r.VoertuigId,
                r.AccountId,

                Voertuig = new
                { 
                    r.Voertuig.Kenteken,
                    r.Voertuig.Merk,
                    r.Voertuig.Model,
                    r.Voertuig.Kleur,
                    r.Voertuig.Aanschafjaar,
                    r.Voertuig.Prijs,
                    r.Voertuig.VoertuigStatus,
                    r.Voertuig.VoertuigType
                }
            });

            var reserveringen = await projectedQuery.ToListAsync();
            if (reserveringen.Count == 0)
                return NotFound(new { Message = "Geen reserveringen gevonden" });

            return Ok(reserveringen);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
    [HttpPost("Reserveer")]
    public async Task<ActionResult<IEnumerable<Reservering>>> Create([FromBody] ReserveringDto reserveringDto)
    {
        try
        {
            var voertuig = await _context.Voertuigen.FindAsync(reserveringDto.VoertuigId);
            if (voertuig == null) 
                return NotFound("Voertuig niet gevonden.");

            if (_context.Reserveringen.Where(r => reserveringDto.VoertuigId == r.VoertuigId)
                .Any(r => reserveringDto.Begindatum < r.Einddatum && reserveringDto.Einddatum > r.Begindatum))
                return BadRequest(new { Message = "Dit voertuig is al gereserveerd in de opgegeven periode." });
            
            var nieuweReservering = Reservering.MaakReservering(reserveringDto, voertuig);
            voertuig.UpdateVoertuigStatus(VoertuigStatusEnum.Gereserveerd);
        
            _context.Reserveringen.Add(nieuweReservering);
            await _context.SaveChangesAsync();
        
            return Ok($"Voertuig met ID {reserveringDto.VoertuigId} is succesvol gereserveerd van {reserveringDto.Begindatum:yyyy-MM-dd} tot {reserveringDto.Einddatum:yyyy-MM-dd}. {reserveringDto.TotaalPrijs}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
     [HttpPut("Update")]
     public async Task<IActionResult> Update([FromQuery] int id, [FromBody] ReserveringDto reserveringDto)
     {
         try
         { 
             var reservering = await _context.Reserveringen.FindAsync(id);
             if (reservering == null)
                 return NotFound(new { Message = $"Reservering met ID {id} staat niet in de database" });
             
             var voertuig = await _context.Voertuigen.FindAsync(reserveringDto.VoertuigId);
             if (voertuig == null)
                 return NotFound(new { Message = $"Voertuig met ID {reserveringDto.VoertuigId} niet gevonden in de database" });

             var kosten = reservering.UpdateReservering(reserveringDto, voertuig);
             if (kosten < 1)
                 return BadRequest(new { Message = "Geen bijkomende kosten" });
             
             var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == reservering.AccountId);
             if (account == null) 
                 return BadRequest(new { Message = $"Account met ID {reservering.AccountId} staat niet in database" });
             
             await _context.SaveChangesAsync();
             EmailSender.VerstuurWijzigReserveringEmail(account.Email);
             
             return Ok(kosten);
         }
         catch (Exception ex)
         {
             return StatusCode(500, new { ex.Message });
         }
     }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] int reserveringId)
    {
        try
        {
            var reservering = await _context.Reserveringen.FindAsync(reserveringId);
            if (reservering == null)
                return NotFound(new { Message = $"Reservering met ID {reserveringId} staat niet in de database" });

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == reservering.AccountId);
            if (account == null) 
                return NotFound(new { Message = $"Voertuig met ID {account.AccountId} staat niet in de database" });
            
            _context.Reserveringen.Remove(reservering);
            await _context.SaveChangesAsync();
            
            EmailSender.VerstuurVerwijderReserveringEmail(account.Email);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
}
