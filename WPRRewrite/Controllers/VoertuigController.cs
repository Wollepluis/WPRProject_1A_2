using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Voertuigen;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/Voertuig")]
public class VoertuigController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public VoertuigController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet("krijgallevoertuigen")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> GetAlleVoertuigen()
    {
        var Voertuigen = await _context.Voertuigen.Include(a => a.Reserveringen).ToListAsync();
        return Ok(Voertuigen);
    }
    
    [HttpGet("krijgallevoertuigenDatum")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> GetAlleVoertuigen(DateTime begindatum, DateTime einddatum)
    {
        var Voertuigen = await _context.Voertuigen.ToListAsync();
        List<IVoertuig> beschikbareVoertuigen = new List<IVoertuig>();
        foreach (var voertuig in Voertuigen)
        {
            var reserveringen = voertuig.GetReserveringen();
            foreach (var reservering in reserveringen)
            {
                if ((begindatum < reservering.Einddatum && einddatum > reservering.Begindatum))
                {
                    beschikbareVoertuigen.Add(voertuig);
                }
            }
        }
        return Ok(beschikbareVoertuigen);
    }

    [HttpGet("krijgspecifiekvoertuig")]
    public async Task<ActionResult<Voertuig>> GetVoertuig(int id)
    {
        var voertuig = await _context.Voertuigen.FindAsync(id);

        if (voertuig == null)
        {
            return NotFound();
        }
        return Ok(voertuig);
    }

    [HttpGet("Filter voertuigen")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> FilterVoertuigen(string voertuigType)
    {
        if (string.IsNullOrWhiteSpace(voertuigType))
        {
            return BadRequest("VoertuigType is verplicht meegegeven te worden");
        }
        
        var voertuigen = await _context.Voertuigen
            .Where(v => EF.Property<string>(v, "VoertuigType") == voertuigType)
            .ToListAsync();

        return Ok(voertuigen);
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
    
    [HttpPost("reserveerVoertuig")]
    public async Task<IActionResult> ReserveerVoertuig(int id, int accountId, DateTime begindatum, DateTime einddatum)
    {
        // Zoek het voertuig in de database
        var voertuig = await _context.Voertuigen.FindAsync(id);

        if (voertuig == null) return NotFound("Voertuig niet gevonden.");

        // Haal bestaande reserveringen op
        var reserveringen = voertuig.GetReserveringen() ?? new List<Reservering>();

        // Controleer of er overlap is met bestaande reserveringen
        if (reserveringen.Any(r => begindatum < r.Einddatum && einddatum > r.Begindatum))
            return BadRequest("Dit voertuig is al gereserveerd in de opgegeven periode.");

        // Maak een nieuwe reservering aan
        var reserveringDto = new Reservering(
            begindatum,
            einddatum,
            100 * ((einddatum - begindatum).Days), // Bereken de kosten
            voertuig.VoertuigId,
            accountId
        );

        // Voeg de reservering toe
        _context.Reserveringen.Add(reserveringDto);

        // Sla wijzigingen op in de database
        await _context.SaveChangesAsync();

        return Ok($"Voertuig met ID {id} is succesvol gereserveerd van {begindatum:yyyy-MM-dd} tot {einddatum:yyyy-MM-dd}.");
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
    
    [HttpPut("HuuraanvraagUpdaten")]
    public async Task<IActionResult> HuuraanvraagUpdaten(int id ,string status)
    {
        var voertuig = await _context.Voertuigen.FindAsync(id);
        if (voertuig == null) return NotFound();
        voertuig.VoertuigStatus = status;
        return NoContent();
    }
    
}

