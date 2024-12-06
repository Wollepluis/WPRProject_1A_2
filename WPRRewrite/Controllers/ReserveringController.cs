using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ReserveringController(CarAndAllContext context) : ControllerBase
{
    [HttpGet("PerAccount/{accountId}")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringenPerAccount(int accountId)
    {
        var reserveringen = await context.Reserveringen.Where(r => r.Account == accountId).ToListAsync();
        if (reserveringen.Count == 0)
        {
            return NotFound($"Geen reserveringen gevonden voor accountId: {accountId}");
        }
        return Ok(reserveringen);
    }

    [HttpGet("PerVoertuig/{voertuigId}")]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringenPerVoertuig(int voertuigId)
    {
        var voertuig = await context.Voertuigen.FindAsync(voertuigId);
        if (voertuig == null) return NotFound();
        voertuig.GetReserveringen();
        return Ok(voertuig);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<Reservering>>> PostReservering(Reservering reservering)
    {
        var zakelijkAccount = await context.ZakelijkAccounts.FindAsync(reservering.Account);
        if (zakelijkAccount == null) return NotFound();
        zakelijkAccount.AddReservering(reservering);
        
        context.Reserveringen.Add(reservering);
        
        for (int i = 0; reservering.GereserveerdeVoertuigen.Count() > i; i++)
        {
            var voertuig = reservering.GereserveerdeVoertuigen.ElementAt(i);
            voertuig.Reserveringen.Add(reservering);
        }
        
        await context.SaveChangesAsync();
        
        return Ok(reservering);
    }

    [HttpPut]
    public async Task<ActionResult<IEnumerable<Reservering>>> UpdateReservering(int id, Reservering geupdateReservering)
    {
        var bestaandeReservering = await context.Reserveringen.FindAsync(id);
        if (bestaandeReservering == null) return NotFound();
        bestaandeReservering.Update(geupdateReservering);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult<IEnumerable<Reservering>>> DeleteReservering(int id)
    {
        var reservering = await context.Reserveringen.FindAsync(id);
        if (reservering == null) return NotFound();
        context.Reserveringen.Remove(reservering);
        await context.SaveChangesAsync();
        return NoContent();
    }
}