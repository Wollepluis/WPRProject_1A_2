using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WPRRewrite.Modellen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AdresController(CarAndAllContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Adres>>> GetAlleAdressen()
    {
        return await context.Adressen.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Adres>> GetAdres(int id)
    {
        var adres = await context.Adressen.FindAsync(id);

        if (adres == null)
        {
            return NotFound();
        }
        return Ok(adres);
    }
    
    [HttpPost]
    public async Task<ActionResult<Adres>> PostAdres([FromBody] Adres adres)
    {
        if (adres == null)
        {
            return BadRequest("Adres mag niet 'NULL' zijn");
        }

        context.Adressen.Add(adres);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAdres), new { id = adres.AdresId }, adres);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdres(int id, [FromBody] Adres updatedAdres)
    {
        if (id != updatedAdres.AdresId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAdres = await context.Adressen.FindAsync(id);
        if (existingAdres == null)
        {
            return NotFound();
        }

        existingAdres.UpdateAdres(updatedAdres);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdres(int id)
    {
        var adres = await context.Adressen.FindAsync(id);
        if (adres == null)
        {
            return NotFound();
        }

        context.Adressen.Remove(adres);
        await context.SaveChangesAsync();

        return NoContent();
    }
}