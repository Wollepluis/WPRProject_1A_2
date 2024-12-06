using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Abonnementen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AbonnementController(CarAndAllContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Abonnement>>> GetAlleAbonnementen()
    {
        return await context.Abonnementen.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Abonnement>> GetAbonnement(int id)
    {
        var abonnement = await context.Abonnementen.FindAsync(id);

        if (abonnement == null)
        {
            return NotFound();
        }
        return Ok(abonnement);
    }

    [HttpPost]
    public async Task<ActionResult<Abonnement>> PostAbonnement([FromBody] Abonnement abonnement)
    {
        if (abonnement == null)
        {
            return BadRequest("Abonnement mag niet 'NULL' zijn");
        }
        
        context.Abonnementen.Add(abonnement);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAbonnement), new { id = abonnement.AbonnementId }, abonnement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAbonnement(int id, [FromBody] Abonnement updatedAbonnement)
    {
        if (id != updatedAbonnement.AbonnementId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAbonnement = await context.Abonnementen.FindAsync(id);
        if (existingAbonnement == null)
        {
            return NotFound();
        }

        existingAbonnement.UpdateAbonnement(updatedAbonnement);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAbonnement(int id)
    {
        var abonnement = await context.Abonnementen.FindAsync(id);
        if (abonnement == null)
        {
            return NotFound();
        }

        context.Abonnementen.Remove(abonnement);
        await context.SaveChangesAsync();

        return NoContent();
    }
}