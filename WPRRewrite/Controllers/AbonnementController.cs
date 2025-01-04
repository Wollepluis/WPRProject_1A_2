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
    public async Task<ActionResult> CreateAbonnement([FromBody] Abonnement abonnement, int bedrijfId)
    {
        if (abonnement == null)
            return BadRequest("Abonnement gegevens zijn niet ingevuld.");

        var bedrijf = await _context.Bedrijven.FindAsync(bedrijfId);
        if (bedrijf == null)
            return NotFound("Bedrijf niet gevonden.");

        _context.Abonnementen.Add(abonnement);
        await _context.SaveChangesAsync();

        bedrijf.AbonnementId = abonnement.AbonnementId;
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAbonnementById), new { id = abonnement.AbonnementId }, abonnement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAbonnement(int id, [FromBody] Abonnement updatedAbonnement, int bedrijfId)
    {
        if (id != updatedAbonnement.AbonnementId)
            return BadRequest("ID mismatch.");

        var existingAbonnement = await _context.Abonnementen.FindAsync(id);
        if (existingAbonnement == null)
            return NotFound("Abonnement niet gevonden.");

        var bedrijf = await _context.Bedrijven.FindAsync(bedrijfId);
        if (bedrijf == null)
            return NotFound("Bedrijf niet gevonden.");

        existingAbonnement.MaxVoertuigen = updatedAbonnement.MaxVoertuigen;
        existingAbonnement.MaxMedewerkers = updatedAbonnement.MaxMedewerkers;
        //existingAbonnement.Begindatum = updatedAbonnement.Begindatum;

        bedrijf.AbonnementId = existingAbonnement.AbonnementId;
        await _context.SaveChangesAsync();

        return NoContent();
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
