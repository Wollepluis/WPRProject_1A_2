using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class CaravanController : ControllerBase, IVoertuigController
{
    private readonly CarAndAllContext _context;

    public CaravanController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet("Krijg alle voertuigen")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> GetAlleVoertuigen()
    {
        List<CaravanDto> caravanDtos = new List<CaravanDto>();
        List<Voertuig> voertuigen = await FilterVoertuigen("Caravan");
        if (voertuigen == null) return NoContent();
        foreach (Voertuig voertuig in voertuigen)
        {
            CaravanDto caravanDto = new CaravanDto(voertuig.Kenteken, voertuig.Merk, voertuig.Model, voertuig.Kleur, voertuig.Aanschafjaar, voertuig.Prijs, voertuig.VoertuigStatus);
            caravanDtos.Add(caravanDto);
        }
        return Ok(caravanDtos);
    }

    [HttpGet("Krijg specifiek voertuig / {id}")]
    public async Task<ActionResult<IVoertuig>> GetVoertuig(int id)
    {
        IVoertuig caravan = await _context.Voertuigen.FindAsync(id);
        
        if (caravan == null)
        {
            return NotFound();
        }
        CaravanDto caravanDto = new CaravanDto(caravan.Kenteken, caravan.Merk, caravan.Model, caravan.Kleur, caravan.Aanschafjaar, caravan.Prijs, caravan.VoertuigStatus);

        return Ok(caravanDto);
    }

    [HttpPost("Voeg Caravan Toe")]
    public async Task<ActionResult<VoertuigDto>> PostVoertuig(VoertuigDto caravanDto) 
    {
        if (caravanDto == null)
        {
            return BadRequest("Voertuig mag niet 'NULL' zijn");
        }
        Caravan caravan = new Caravan(caravanDto.Kenteken, caravanDto.Merk, caravanDto.Model, caravanDto.Kleur, caravanDto.Aanschafjaar, caravanDto.Prijs, "Beschikbaar");
        _context.Voertuigen.Add(caravan);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVoertuig), new { id = caravan.VoertuigId }, caravan);
    }

    [HttpPut("Update Caravan / {id}")]
    public async Task<IActionResult> PutVoertuig(int id, IVoertuigDto updatedCaravanDto)
    {
        IVoertuig? bestaandeCaravan = await _context.Voertuigen.FindAsync(id);
        
        if (bestaandeCaravan == null) return NotFound();

        Caravan updatedCaravan = new Caravan(updatedCaravanDto.Kenteken, updatedCaravanDto.Merk, updatedCaravanDto.Model, updatedCaravanDto.Kleur, updatedCaravanDto.Aanschafjaar, updatedCaravanDto.Prijs, updatedCaravanDto.VoertuigStatus);
        
        bestaandeCaravan.UpdateVoertuig(updatedCaravan);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("Delete Caravan / {id}")]
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
    
    [HttpGet("filter voertuigen")]
    public async Task<List<Voertuig>> FilterVoertuigen(string voertuigType)
    {
        if (string.IsNullOrWhiteSpace(voertuigType))
        {
            return null;
        }

        var voertuigen = await _context.Voertuigen
            .Where(v => EF.Property<string>(v, "VoertuigType") == voertuigType)
            .ToListAsync();

        return voertuigen;
    }
}