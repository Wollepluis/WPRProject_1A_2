using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen.Voertuigen;
using WPRRewrite.SysteemFuncties;

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
        
        
        ReserveringVoertuigDto reserveringVoertuigDto = new ReserveringVoertuigDto(reservering.VoertuigId, reservering.ReserveringId, voertuig.Kenteken, voertuig.Merk, voertuig.Model, voertuig.Kleur, voertuig.Aanschafjaar, voertuig.VoertuigType, voertuig.BrandstofType, reservering.Begindatum, reservering.Einddatum, reservering.TotaalPrijs, reservering.IsBetaald, reservering.IsGoedgekeurd);
        
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

    [HttpGet("KrijgAlleReserveringenGoedgekeurd")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleGoedgekeurdeReserveringen()
    {
        var reserveringen = await _context.Reserveringen
            .Include(r => r.Voertuig)
            .Where(r => r.IsGoedgekeurd == true && r.Einddatum != DateTime.Today && r.Voertuig.VoertuigStatus == "Uitgegeven")
            .ToListAsync();

        if (reserveringen == null)
            return Ok(new { Message = "Geen voertuigen gevonden" });

        return Ok(reserveringen);
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
        Reservering reservering = _context.Reserveringen.Find(reserveringId);
        var voertuig = _context.Voertuigen.Find(voertuigReserveringDto.VoertuigId);
        reservering.Begindatum = voertuigReserveringDto.Begindatum;
        reservering.Einddatum = voertuigReserveringDto.Einddatum;
        reservering.VoertuigId = voertuigReserveringDto.VoertuigId;
        var days = (voertuigReserveringDto.Einddatum - voertuigReserveringDto.Begindatum).Days;
        var bijkomendeKosten = 0;
        if(voertuig.VoertuigType == "Auto")
        {
            bijkomendeKosten = 100 + 100 * days; // Zorg ervoor dat `totaalPrijs` bestaat
        } else if (voertuig.VoertuigType == "Caravan")
        {
            bijkomendeKosten = 200 + 200 * days; // Zorg ervoor dat `totaalPrijs` bestaat
        } else if (voertuig.VoertuigType == "Camper")
        {
            bijkomendeKosten = 300 + 300 * days; // Zorg ervoor dat `totaalPrijs` bestaat
        }
        if (bijkomendeKosten < 1) return BadRequest("Geen bijkomende kosten");
        reservering.TotaalPrijs = bijkomendeKosten;
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == reservering.AccountId);
        if (account != null)
        {
            EmailSender.VerstuurWijzigReserveringEmail(account.Email);
            await _context.SaveChangesAsync();    
        }
        
        return Ok(bijkomendeKosten);
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

        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == reservering.AccountId);
        if (account != null)
        {
            EmailSender.VerstuurVerwijderReserveringEmail(account.Email);
            await _context.SaveChangesAsync();
        }

        return NoContent();
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
    }
}
