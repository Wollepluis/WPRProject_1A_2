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
        [FromQuery] int? accountId)
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
            
            if (accountId.HasValue)
            {
                query = query.Where(r => r.AccountId == accountId);
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
                return NotFound(new { Message = "Geen reserveringen met dit type gevonden" });

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
    
    // Kijk of deze klopt (UpdateReservering + berekening)
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
    
    //new
    /*[HttpGet("GetAlleVoertuigenMetReserveringen")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> GetAlleVoertuigenMetReserveringen()
    {
        // Haal alle reserveringen op
        var reserveringen = await _context.Reserveringen
            .Where(r => r.IsGoedgekeurd == false)
            .ToListAsync();
        
        // Controleer of er reserveringen zijn
        if (reserveringen.Count == 0)
        {
            return Ok(new { Message = "Er zijn geen reserveringen gevonden." });
        }

        List<ReserveringVoertuigDto> alleReserveringen = new List<ReserveringVoertuigDto>();

        foreach (var reservering in reserveringen)
        {
            // Zoek het voertuig op basis van de VoertuigId van de reservering
            var voertuig = await _context.Voertuigen.FindAsync(reservering.VoertuigId);

            // Als het voertuig niet gevonden kan worden, geef een foutmelding
            if (voertuig == null) return NotFound();
            ReserveringVoertuigDto reservering2 = new ReserveringVoertuigDto(reservering.VoertuigId, reservering.ReserveringId, voertuig.Kenteken, voertuig.Merk, voertuig.Model, voertuig.Kleur, voertuig.Aanschafjaar, voertuig.VoertuigType, voertuig.BrandstofType, reservering.Begindatum, reservering.Einddatum, reservering.TotaalPrijs, reservering.IsBetaald, reservering.IsGoedgekeurd);
            alleReserveringen.Add(reservering2);
        }

        // Retourneer de lijst van reserveringen met voertuigen
        return Ok(alleReserveringen);
    }
    
    [HttpGet("KrijgAlleReserveringenFrontoffice")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringen()
    {
        var reserveringen = await _context.Reserveringen.ToListAsync();
        var reserveringenDto = new List<FrontofficeReserveringDto>();
        foreach (var reservering in reserveringen) 
        {
            
                // Handle null value scenario, e.g. set default values or log error
                
            var account = await _context.Accounts.FindAsync(reservering.AccountId);
            var voertuig = await _context.Voertuigen.FindAsync(reservering.VoertuigId);
            if (account == null || voertuig == null) return BadRequest("Er is iets fout gegaan met het ophalen van een reservering");
            FrontofficeReserveringDto frontofficeReserveringDto = new FrontofficeReserveringDto(reservering.ReserveringId, voertuig.Kenteken, voertuig.Merk, voertuig.Model, voertuig.Kleur, voertuig.Aanschafjaar, voertuig.VoertuigType, voertuig.BrandstofType, reservering.Begindatum, reservering.Einddatum, reservering.TotaalPrijs, reservering.IsBetaald, reservering.IsGoedgekeurd, account.Email);
            reserveringenDto.Add(frontofficeReserveringDto);
        }
        return Ok(reserveringenDto);
    }*/
    
    /*[HttpGet("KrijgAlleReserveringenPerVoertuig")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringenPerVoertuig(int voertuigId)
    {
        var voertuig = await _context.Voertuigen.FindAsync(voertuigId);
        if (voertuig == null) return NotFound();
        voertuig.GetReserveringen();
        return Ok(voertuig);
    }*/
    
    /*[HttpGet("KrijgGehuurdeBedrijfsvoertuigen")]
    public async Task<ActionResult<Voertuig>> GetVoertuig(List<AccountZakelijk> medewerkers)
    {
        var reserveringen = new List<Reservering>();
        foreach (var medewerker in medewerkers)
        {
            foreach (var reservering in medewerker.Reserveringen)
            {
                reserveringen.Add(reservering);
            }
        }
        return Ok(reserveringen);
    }
    
    //Nieuw
    [HttpGet("KrijgGehuurdeBedrijfsreserveringen")]
    public async Task<ActionResult<Voertuig>> GetReserveringen(int bedrijfsId)
    {
        var accounts = await _context.Accounts.OfType<AccountZakelijk>().Where(b => b.BedrijfId == bedrijfsId).ToListAsync();
        var reserveringen = new List<Reservering>();
        foreach (var medewerker in accounts)
        {
            var reservering = await _context.Reserveringen.Where(r => r.Account.AccountId == medewerker.AccountId).Include(a => a.Voertuig).Include(a => a.Account).ToListAsync();
            reserveringen.AddRange(reservering);
        }
        var gesorteerdeReserveringen = reserveringen.OrderBy(a => a.Begindatum);
        return Ok(gesorteerdeReserveringen);
    }*/
}
