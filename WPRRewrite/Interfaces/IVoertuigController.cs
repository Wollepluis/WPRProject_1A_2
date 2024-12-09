using Microsoft.AspNetCore.Mvc;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Interfaces;

public interface IVoertuigController
{
    public Task<ActionResult<IEnumerable<IVoertuig>>> GetAlleVoertuigen();
    public Task<ActionResult<IVoertuig>> GetVoertuig(int id);
    public Task<List<Voertuig>> FilterVoertuigen(string voertuigType);
    public Task<ActionResult<VoertuigDto>> PostVoertuig(VoertuigDto voertuig);
    public Task<IActionResult> PutVoertuig(int id, IVoertuigDto updatedCaravanDto);
    public Task<IActionResult> DeleteVoertuig(int id);
}