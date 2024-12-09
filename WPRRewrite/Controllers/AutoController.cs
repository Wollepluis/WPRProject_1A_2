using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AutoController : ControllerBase, IVoertuigController
{
    private readonly CarAndAllContext _context;

    public AutoController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet("Krijg alle Auto voertuigen")]
    public async Task<ActionResult<IEnumerable<VoertuigDto>>> GetAlleVoertuigen()
    {
        List<VoertuigDto> voertuigen = await FilterVoertuigen("Auto");
        if (voertuigen == null || !voertuigen.Any()) return NoContent();
        return Ok(voertuigen);
    }

    [HttpGet("Krijg specifiek Auto voertuig / {id}")]
    public async Task<ActionResult<VoertuigDto>> GetVoertuig(int id)
    {
        IVoertuig auto = await _context.Voertuigen.FindAsync(id);
        if (auto == null) return NotFound();
        
        AutoDto autoDto = new AutoDto(auto.Kenteken, auto.Merk, auto.Model, auto.Kleur, auto.Aanschafjaar, auto.Prijs, auto.VoertuigStatus);

        return Ok(autoDto);
    }

    [HttpPost("Voeg Auto Toe")]
    public async Task<ActionResult<VoertuigDto>> PostVoertuig(VoertuigDto camperDto) 
    {
        if (camperDto == null)
        {
            return BadRequest("Voertuig mag niet 'NULL' zijn");
        }
        Auto auto = new Auto(camperDto.Kenteken, camperDto.Merk, camperDto.Model, camperDto.Kleur, camperDto.Aanschafjaar, camperDto.Prijs, "Beschikbaar");
        _context.Voertuigen.Add(auto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVoertuig), new { id = auto.VoertuigId }, auto);
    }

    [HttpPut("Update Auto / {id}")]
    public async Task<IActionResult> PutVoertuig(int id, VoertuigDto updatedCamperDto)
    {
        IVoertuig? bestaandeAuto = await _context.Voertuigen.FindAsync(id);
        
        if (bestaandeAuto == null) return NotFound();

        Auto updatedAuto = new Auto(updatedCamperDto.Kenteken, updatedCamperDto.Merk, updatedCamperDto.Model, updatedCamperDto.Kleur, updatedCamperDto.Aanschafjaar, updatedCamperDto.Prijs, updatedCamperDto.VoertuigStatus);
        
        bestaandeAuto.UpdateVoertuig(updatedAuto);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("Delete Auto / {id}")]
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
    
    [HttpGet("filter voertuigen (Auto)")]
    public async Task<List<VoertuigDto>> FilterVoertuigen(string voertuigType)
    {
        if (string.IsNullOrWhiteSpace(voertuigType)) return null;

        var voertuigen = await _context.Voertuigen
            .Where(v => EF.Property<string>(v, "VoertuigType") == voertuigType)
            .ToListAsync();

        List<VoertuigDto> voertuigDtos = new List<VoertuigDto>();
        foreach (var voertuig in voertuigen)
        {
            AutoDto voertuigDto = new AutoDto(voertuig.Kenteken, voertuig.Merk, voertuig.Model, voertuig.Kleur, voertuig.Aanschafjaar, voertuig.Prijs, voertuig.VoertuigStatus);
            voertuigDtos.Add(voertuigDto);
        }

        return voertuigDtos;
    }

}