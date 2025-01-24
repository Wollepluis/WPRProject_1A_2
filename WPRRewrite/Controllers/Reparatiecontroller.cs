using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ReparatieController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public ReparatieController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet("Krijg alle reparaties")]
    public async Task<ActionResult<IEnumerable<Reparatie>>> GetAllReparaties()
    {
        return await _context.Reparaties.OfType<Reparatie>().ToListAsync();
    }

    [HttpGet("KrijgSpecifiekeReparatie")]
    public async Task<ActionResult<Reparatie>> GetReparatie(int id)
    {
        var reparatie = await _context.Reparaties.FindAsync(id);

        if (reparatie == null)
        {
            return NotFound();
        }
        return Ok(reparatie);
    }

    [HttpPost("MaakReparatieAan")]
    public async Task<ActionResult<Reparatie>> PostReparatie(int schadeclaimId, [FromBody] Reparatie reparatie)
    {
        if (reparatie == null)
        {
            return BadRequest("Reparatie mag niet 'NULL' zijn");
        }
        
        _context.Reparaties.Add(reparatie);
        await _context.SaveChangesAsync();
        
        var schadeclaim = await _context.Schadeclaim.FindAsync(schadeclaimId);
        if (schadeclaim == null) return BadRequest("Schadeclaim niet gevonden");
        
        schadeclaim.ReparatieId = reparatie.ReparatieId;
        
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetReparatie), new { id = reparatie.ReparatieId }, reparatie);
    }
}