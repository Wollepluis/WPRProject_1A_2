using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class CamperController : ControllerBase, IVoertuigController
{
    private readonly Context _context;

    public CamperController(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet("Krijg alle voertuigen")]
    public async Task<ActionResult<IEnumerable<VoertuigDto>>> GetAlleVoertuigen()
    {
        List<VoertuigDto> voertuigen = await FilterVoertuigen("Camper");
        if (voertuigen == null || !voertuigen.Any()) return NoContent();
        return Ok(voertuigen);
    }

    [HttpGet("Krijg specifiek voertuig / {id}")]
    public async Task<ActionResult<VoertuigDto>> GetVoertuig(int id)
    {
        IVoertuig camper = await _context.Voertuigen.FindAsync(id);
        if (camper == null) return NotFound();
        
        CamperDto camperDto = new CamperDto(camper.Kenteken, camper.Merk, camper.Model, camper.Kleur, camper.Aanschafjaar, camper.Prijs, camper.VoertuigStatus, camper.BrandstofType);

        return Ok(camperDto);
    }

    [HttpPost("Voeg Camper Toe")]
    public async Task<ActionResult<VoertuigDto>> PostVoertuig(VoertuigDto camperDto) 
    {
        if (camperDto == null)
        {
            return BadRequest("Voertuig mag niet 'NULL' zijn");
        }
        Camper camper = new Camper(camperDto.Kenteken, camperDto.Merk, camperDto.Model, camperDto.Kleur, camperDto.Aanschafjaar, camperDto.Prijs, "Beschikbaar", camperDto.BrandstofType);
        _context.Voertuigen.Add(camper);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVoertuig), new { id = camper.VoertuigId }, camper);
    }

    [HttpPut("Update Camper / {id}")]
    public async Task<IActionResult> PutVoertuig(int id, VoertuigDto updatedCamperDto)
    {
        IVoertuig? bestaandeCamper = await _context.Voertuigen.FindAsync(id);
        
        if (bestaandeCamper == null) return NotFound();

        Camper updatedCamper = new Camper(updatedCamperDto.Kenteken, updatedCamperDto.Merk, updatedCamperDto.Model, updatedCamperDto.Kleur, updatedCamperDto.Aanschafjaar, updatedCamperDto.Prijs, updatedCamperDto.VoertuigStatus, updatedCamperDto.BrandstofType);
        
        bestaandeCamper.UpdateVoertuig(updatedCamper);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("Delete Camper / {id}")]
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
    public async Task<List<VoertuigDto>> FilterVoertuigen(string voertuigType)
    {
        if (string.IsNullOrWhiteSpace(voertuigType)) return null;

        var voertuigen = await _context.Voertuigen
            .Where(v => EF.Property<string>(v, "VoertuigType") == voertuigType)
            .ToListAsync();

        List<VoertuigDto> voertuigDtos = new List<VoertuigDto>();
        foreach (var voertuig in voertuigen)
        {
            CamperDto voertuigDto = new CamperDto(voertuig.Kenteken, voertuig.Merk, voertuig.Model, voertuig.Kleur, voertuig.Aanschafjaar, voertuig.Prijs, voertuig.VoertuigStatus, voertuig.BrandstofType);
            voertuigDtos.Add(voertuigDto);
        }

        return voertuigDtos;
    }

}