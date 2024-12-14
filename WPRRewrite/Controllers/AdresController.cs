using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AdresController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public AdresController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Adres>>> GetAlleAdressen()
    {
        return await _context.Adressen.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Adres>> GetAdres(int id)
    {
        var adres = await _context.Adressen.FindAsync(id);

        if (adres == null) return NotFound();
        return Ok(adres);
    }
    
    [HttpPost]
    public async Task<ActionResult<Adres>> PostAdres([FromBody] Adres adres)
    {
        
        if (adres == null)
        {
            return BadRequest("Adres mag niet 'NULL' zijn");
        }

        _context.Adressen.Add(adres);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAdres), new { id = adres.AdresId }, adres);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdres(int id, [FromBody] Adres updatedAdres)
    {
        if (id != updatedAdres.AdresId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAdres = await _context.Adressen.FindAsync(id);
        if (existingAdres == null)
        {
            return NotFound();
        }

        existingAdres.UpdateAdres(updatedAdres);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdres(int id)
    {
        var adres = await _context.Adressen.FindAsync(id);
        if (adres == null)
        {
            return NotFound();
        }

        _context.Adressen.Remove(adres);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}