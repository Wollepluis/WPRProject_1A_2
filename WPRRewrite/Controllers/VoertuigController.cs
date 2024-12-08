using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class VoertuigController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public VoertuigController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> GetAlleVoertuigen()
    {
        return await _context.Voertuigen.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Voertuig>> GetVoertuig(int id)
    {
        var voertuig = await _context.Voertuigen.FindAsync(id);

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
        
        _context.Voertuigen.Add(voertuig);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVoertuig), new { id = voertuig.VoertuigId }, voertuig);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutVoertuig(int id, [FromBody] Voertuig updatedVoertuig)
    {
        if (id != updatedVoertuig.VoertuigId)
        {
            return BadRequest("ID mismatch");
        }

        var existingVoertuig = await _context.Voertuigen.FindAsync(id);
        if (existingVoertuig == null)
        {
            return NotFound();
        }

        existingVoertuig.UpdateVoertuig(updatedVoertuig);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVoertuig(int id)
    {
        var voertuig = await _context.Voertuigen.FindAsync(id);
        if (voertuig == null)
        {
            return NotFound();
        }

        _context.Voertuigen.Remove(voertuig);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}