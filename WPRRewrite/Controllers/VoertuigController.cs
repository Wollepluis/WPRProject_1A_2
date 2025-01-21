using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("[Controller]")]
public class VoertuigController(Context context) : ControllerBase
{
    private readonly Context _context = context ?? throw new ArgumentNullException(nameof(context));
    
    [HttpGet("GetVoertuigen")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> GetVoertuigen([FromQuery] string? voertuigType, 
        [FromQuery] int? voertuigId, [FromQuery] DateOnly? begindatum, [FromQuery] DateOnly? einddatum)
    {
        try
        {
            IQueryable<IVoertuig> query = _context.Voertuigen;
            
            if (voertuigId.HasValue)
            {
                var voertuig = await query.FirstOrDefaultAsync(v => v.VoertuigId == voertuigId);
                if (voertuig == null)
                    return NotFound(new { Message = $"Voertuig met ID {voertuigId} niet gevonden" });

                return Ok(voertuig);
            }

            if (voertuigType.IsNullOrEmpty())
            {
                query = voertuigType switch
                {
                    "Auto" => query.OfType<Auto>(),
                    "Camper" => query.OfType<Camper>(),
                    "Caravan" => query.OfType<Caravan>(),
                    _ => throw new ArgumentOutOfRangeException(nameof(voertuigType), voertuigType, "Onjuist voertuig type")
                };
            }

            if (begindatum.HasValue && einddatum.HasValue)
            {
                query = query.Where(v => !_context.Reserveringen
                    .Any(r => r.VoertuigId == v.VoertuigId &&
                              begindatum <= r.Einddatum &&
                              einddatum >= r.Begindatum));
            }

            var voertuigen = await query.ToListAsync();
            if (voertuigen.Count == 0)
                return NotFound(new { Message = "Geen voertuigen met dit type gevonden" });

            return Ok(voertuigen);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }

    [HttpPost("AddVoertuig")]
    public async Task<ActionResult<IVoertuig>> Create([FromBody] VoertuigDto voertuigDto)
    {
        try
        {
            var checkVoertuig =
                _context.Voertuigen.Any(v => v.Merk == voertuigDto.Merk && v.Model == voertuigDto.Model);
            if (checkVoertuig)
                return BadRequest(new { Message = "Een voertuig met dit merk en model bestaat al" });

            var nieuwVoertuig = Voertuig.MaakVoertuig(voertuigDto);
            
            _context.Voertuigen.Add(nieuwVoertuig);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Voertuig toegevoegd aan de database" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }

    
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromQuery] int accountId, [FromBody] VoertuigDto nieuweGegevens)
    {
        try
        {
            var voertuig = await _context.Voertuigen.FindAsync(accountId);
            if (voertuig == null)
                return NotFound(new { Message = $"Voertuig met ID {accountId} staat niet in de database" });
            
            voertuig.UpdateVoertuig(nieuweGegevens);

            return Ok(new { Message = "Voertuig succesvol aangepast" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] int voertuigId)
    {
        try {
            var voertuig = await _context.Voertuigen.FindAsync(voertuigId);
            if (voertuig == null)
                return NotFound(new { Message = $"Voertuig met ID {voertuigId} staat niet in de database" });

            _context.Voertuigen.Remove(voertuig);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Voertuig {voertuig.Merk} {voertuig.Model} succesvol verwijderd" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
}

