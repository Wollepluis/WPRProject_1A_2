using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BedrijfController(CarAndAllContext context) : ControllerBase
{
    private IAdresService _adresService;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bedrijf>>> GetAlleBedrijven()
    {
        return await context.Bedrijven.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bedrijf>> GetBedrijf(int id)
    {
        var bedrijf = await context.Bedrijven.FindAsync(id);

        if (bedrijf == null)
        {
            return NotFound();
        }
        return Ok(bedrijf);
    }

    [HttpPost]
    public async Task<ActionResult<Bedrijf>> PostBedrijf([FromBody] Bedrijf bedrijf, string postcode, int huisnummer)
    {
        if (bedrijf == null)
        {
            return BadRequest("Bedrijf mag niet 'NULL' zijn");
        }

        var adres = await _adresService.ZoekAdresAsync(postcode, huisnummer);

        if (adres == null)
        {
            return NotFound("Address not found for the given postcode and huisnummer.");
        }

        bedrijf.BedrijfAdres = adres.AdresId;

        context.Bedrijven.Add(bedrijf);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBedrijf), new { id = bedrijf.BedrijfId }, bedrijf);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBedrijf(int id, [FromBody] Bedrijf updatedBedrijf)
    {
        if (id != updatedBedrijf.BedrijfId)
        {
            return BadRequest("ID mismatch");
        }

        var existingBedrijf = await context.Bedrijven.FindAsync(id);
        if (existingBedrijf == null)
        {
            return NotFound();
        }

        existingBedrijf.UpdateBedrijf(updatedBedrijf);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBedrijf(int id)
    {
        var bedrijf = await context.Bedrijven.FindAsync(id);
        if (bedrijf == null)
        {
            return NotFound();
        }

        context.Bedrijven.Remove(bedrijf);
        await context.SaveChangesAsync();

        return NoContent();
    }
}