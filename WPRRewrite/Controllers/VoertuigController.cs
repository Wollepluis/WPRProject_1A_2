using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class VoertuigController(CarAndAllContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Voertuig>>> GetAlleVoertuigen()
    {
        return await context.Voertuigen.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Voertuig>> GetVoertuig(int id)
    {
        var voertuig = await context.Voertuigen.FindAsync(id);

        if (voertuig == null)
        {
            return NotFound();
        }
        return Ok(voertuig);
    }

    [HttpPost]
    public async Task<ActionResult<Voertuig>> PostVoertuig([FromBody] Voertuig voertuig)
    {
        if (voertuig == null)
        {
            return BadRequest("Voertuig mag niet 'NULL' zijn");
        }

        voertuig.CreateVoertuigAsync(voertuig.TypeVoertuig);
        
        context.Voertuigen.Add(voertuig);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVoertuig), new { id = voertuig.VoertuigId }, voertuig);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutVoertuig(int id, [FromBody] Voertuig updatedVoertuig)
    {
        if (id != updatedVoertuig.VoertuigId)
        {
            return BadRequest("ID mismatch");
        }

        var existingVoertuig = await context.Voertuigen.FindAsync(id);
        if (existingVoertuig == null)
        {
            return NotFound();
        }

        existingVoertuig.UpdateVoertuig(updatedVoertuig);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVoertuig(int id)
    {
        var voertuig = await context.Voertuigen.FindAsync(id);
        if (voertuig == null)
        {
            return NotFound();
        }

        context.Voertuigen.Remove(voertuig);
        await context.SaveChangesAsync();

        return NoContent();
    }
}