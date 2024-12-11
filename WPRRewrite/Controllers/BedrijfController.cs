using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
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
    public async Task<ActionResult<BedrijfDto>> GetBedrijf(int id, int kvknummer)
    {
        Bedrijf bedrijf = await _context.Bedrijven.FindAsync(id);

        if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
        if (bedrijf.KvkNummer != kvknummer) return BadRequest("Kvknummer komt niet overeen...");
        
        BedrijfDto bedrijfDto = new BedrijfDto(bedrijf.KvkNummer, bedrijf.Bedrijfsnaam, bedrijf.BedrijfAdres, bedrijf.Domeinnaam);
        return Ok(bedrijf);
    }

    [HttpPost]
    public async Task<ActionResult<Bedrijf>> PostBedrijf([FromBody] BedrijfDto bedrijfDto, string postcode, int huisnummer)
    {
        if (bedrijfDto == null) return BadRequest("Bedrijf moet ingevuld zijn!");

        var adres = await _adresService.ZoekAdresAsync(postcode, huisnummer);

        if (adres == null) return NotFound("Het adres is niet gevonden met de bijbehorende postcode en huisnummer...");
        
        Bedrijf bedrijf = new Bedrijf(bedrijfDto.Kvknummer, bedrijfDto.Bedrijfsnaam, adres.AdresId, bedrijfDto.AbonnementId, bedrijfDto.Domeinnaam);
        
        _context.Bedrijven.Add(bedrijf);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBedrijf), new { id = bedrijf.BedrijfId }, bedrijf);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBedrijf(int id, [FromBody] BedrijfDto updatedBedrijfDto)
    {
        var existingBedrijf = await _context.Bedrijven.FindAsync(id);
        
        if (existingBedrijf == null) return NotFound("Er is geen bedrijf gevonden...");

        Bedrijf updatedBedrijf = new Bedrijf(updatedBedrijfDto.Kvknummer, updatedBedrijfDto.Bedrijfsnaam, updatedBedrijfDto.AbonnementId, updatedBedrijfDto.BedrijfAdres,updatedBedrijfDto.Domeinnaam);
        existingBedrijf.UpdateBedrijf(updatedBedrijf);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBedrijf(int id, int kvknummer)
    {
        var bedrijf = await _context.Bedrijven.FindAsync(id);
        if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
        if (bedrijf.KvkNummer != kvknummer) return BadRequest("Kvknummer komt niet overeen...");
        
        _context.Bedrijven.Remove(bedrijf);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}