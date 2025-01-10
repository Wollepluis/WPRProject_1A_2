using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AbonnementController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public AbonnementController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Abonnement>>> GetAllAbonnementen()
    {
        var abonnementen = await _context.Abonnementen.ToListAsync();
        return Ok(abonnementen);
    }

    [HttpGet("getSpecifiekAbonnement")]
    public async Task<ActionResult<Abonnement>> GetAbonnementById([FromQuery]int id)
    {
        var account = await _context.Accounts.OfType<AccountZakelijk>().FirstOrDefaultAsync(a => a.AccountId == id);
        if (account == null) return Unauthorized(new { message = "Account is niet gevonden"});
        var bedrijf = await _context.Bedrijven.FirstOrDefaultAsync(b => b.BedrijfId == account.BedrijfId);
        if (bedrijf == null) return NotFound("Bedrijf niet gevondem");
        var abonnement = await _context.Abonnementen.FindAsync(bedrijf.AbonnementId);
        if (abonnement == null)
            return NotFound("Abonnement niet gevonden.");

        return Ok(abonnement);
    }

    [HttpPost("Create")]
    public async Task<ActionResult> CreateAbonnement([FromQuery]int accountId, [FromBody] Abonnement abonnement)
    {
        try
        {
            // Ensure abonnement is not null
            if (abonnement == null)
                return BadRequest("Abonnement gegevens zijn niet ingevuld.");

            // Find the bedrijf from the database
            var account = await _context.Accounts.OfType<AccountZakelijk>()
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
            
            var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
            if (bedrijf == null)
                return NotFound("Bedrijf niet gevonden.");

            // Add the new abonnement to the database
            _context.Abonnementen.Add(abonnement);
            await _context.SaveChangesAsync();

            // Update the bedrijf's abonnementId
            bedrijf.AbonnementId = abonnement.AbonnementId;
            await _context.SaveChangesAsync();

            // Return the newly created abonnement
            return CreatedAtAction(nameof(GetAbonnementById), new { id = abonnement.AbonnementId }, abonnement);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Er is een interne serverfout opgetreden.");
        }
    }

//neiwu
    [HttpPut("UpdateAbonnement")]
    public async Task<IActionResult> UpdateAbonnement(int abonnementId, int accountId, [FromBody] Abonnement updatedAbonnement)
    {
        if (accountId == null) return BadRequest();
        var account = await _context.Accounts.OfType<AccountZakelijk>().FirstOrDefaultAsync(a => a.AccountId == accountId);
        var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
        if (bedrijf == null) return NotFound("Bedrijf niet gevonden.");
        if (abonnementId != bedrijf.AbonnementId)
            return BadRequest("ID mismatch.");
        
        var existingAbonnement = await _context.Abonnementen.FindAsync(abonnementId);
        if (existingAbonnement == null) return NotFound("Abonnement niet gevonden.");

        
        
        //existingAbonnement.AbonnementType = updatedAbonnement.AbonnementType;
        existingAbonnement.MaxVoertuigen = updatedAbonnement.MaxVoertuigen;
        existingAbonnement.MaxMedewerkers = updatedAbonnement.MaxMedewerkers;
        //existingAbonnement.Begindatum = updatedAbonnement.Begindatum;

        bedrijf.AbonnementId = existingAbonnement.AbonnementId;
        await _context.SaveChangesAsync();

        return Ok(existingAbonnement);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAbonnement(int id)
    {
        var abonnement = await _context.Abonnementen.FindAsync(id);
        if (abonnement == null)
            return NotFound("Abonnement niet gevonden.");

        _context.Abonnementen.Remove(abonnement);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
