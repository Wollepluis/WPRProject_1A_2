using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BedrijfController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private readonly IAdresService _adresService;
    public BedrijfController(CarAndAllContext context, IAdresService adresService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _adresService = adresService ?? throw new ArgumentNullException(nameof(adresService));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bedrijf>>> GetAlleBedrijven()
    {
        return await _context.Bedrijven.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bedrijf>> GetBedrijf(int id)
    {
        var bedrijf = await _context.Bedrijven.FindAsync(id);

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

        _context.Bedrijven.Add(bedrijf);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBedrijf), new { id = bedrijf.BedrijfId }, bedrijf);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBedrijf(int id, [FromBody] Bedrijf updatedBedrijf)
    {
        if (id != updatedBedrijf.BedrijfId)
        {
            return BadRequest("ID mismatch");
        }

        var existingBedrijf = await _context.Bedrijven.FindAsync(id);
        if (existingBedrijf == null)
        {
            return NotFound();
        }

        existingBedrijf.UpdateBedrijf(updatedBedrijf);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBedrijf(int id)
    {
        var bedrijf = await _context.Bedrijven.FindAsync(id);
        if (bedrijf == null)
        {
            return NotFound();
        }

        _context.Bedrijven.Remove(bedrijf);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}