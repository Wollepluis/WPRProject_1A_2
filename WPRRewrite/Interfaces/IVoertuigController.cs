using Microsoft.AspNetCore.Mvc;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Interfaces;

public interface IVoertuigController
{
    public Task<ActionResult<IEnumerable<VoertuigDto>>> GetAlleVoertuigen();
    public Task<ActionResult<VoertuigDto>> GetVoertuig(int id);
    public Task<List<VoertuigDto>> FilterVoertuigen(string voertuigType);
    public Task<ActionResult<VoertuigDto>> PostVoertuig(VoertuigDto camperDto);
    public Task<IActionResult> PutVoertuig(int id, VoertuigDto updatedCamperDto);
    public Task<IActionResult> DeleteVoertuig(int id);
}