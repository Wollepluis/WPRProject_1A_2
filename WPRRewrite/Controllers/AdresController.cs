using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("[Controller]")]
public class AdresController(Context context) : ControllerBase
{
    private readonly Context _context = context ?? throw new ArgumentNullException(nameof(context));

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
    
    [HttpPost("MaakAdres")]
    public async Task<ActionResult<Adres>> PostAdres([FromBody] AdresDto adresDto)
    {
        var adres = await _context.Adressen.Where(a => a.Postcode == adresDto.Postcode && a.Huisnummer == adresDto.Huisnummer)
            .FirstOrDefaultAsync();

        if (adres != null)
        {
            return Ok(adres);
        }
        
        adres = await AdresService.ZoekAdresAsync(adresDto.Postcode, adresDto.Huisnummer);
        if (adres == null)
            return NotFound(new { Message = "Adres bestaat niet" });
        
        _context.Adressen.Add(adres);
        await _context.SaveChangesAsync();

        return Ok(adres);
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