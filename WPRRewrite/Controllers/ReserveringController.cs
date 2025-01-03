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
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringen2()
    {
        var reserveringen = await _context.Reserveringen.ToListAsync();
        List<int> account = new List<int>();
        foreach (var reservering in reserveringen)
        {
            account.Add(reservering.AccountId);
        }
        return Ok(account);
    }
    
    [HttpGet("KrijgMijnReservering")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetMijnReservering(int reserveringId)
    {
        var reservering = await _context.Reserveringen.FindAsync(reserveringId);
        var voertuig = await _context.Voertuigen.FindAsync(reservering.VoertuigId);
        
        
        ReserveringVoertuigDto reserveringVoertuigDto = new ReserveringVoertuigDto(reservering.ReserveringId, voertuig.Kenteken, voertuig.Merk, voertuig.Model, voertuig.Kleur, voertuig.Aanschafjaar, voertuig.VoertuigType, voertuig.BrandstofType, reservering.Begindatum, reservering.Einddatum, reservering.TotaalPrijs, reservering.IsBetaald, reservering.IsGoedgekeurd);
        
        return Ok(reserveringVoertuigDto);
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

    [HttpPut("PutReservering")]
    public async Task<IActionResult> UpdateReservering(int reserveringId, VoertuigReservering voertuigReserveringDto)
    {
        if (voertuigReserveringDto == null) return BadRequest();
        var reservering = _context.Reserveringen.Find(reserveringId);
        reservering.Begindatum = voertuigReserveringDto.Begindatum;
        reservering.Einddatum = voertuigReserveringDto.Einddatum;
        reservering.VoertuigId = voertuigReserveringDto.VoertuigId;
        await _context.SaveChangesAsync();
        return NoContent();
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