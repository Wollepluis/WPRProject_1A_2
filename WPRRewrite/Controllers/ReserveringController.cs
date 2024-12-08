using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

public class ReserveringController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public ReserveringController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringenPerAccount(int accountId)
    {
        var reserveringen = await _context.Reserveringen.Where(r => r.Account.AccountId == accountId).ToListAsync();
        if (!reserveringen.Any())
        {
            return NotFound($"Geen reserveringen gevonden voor accountId: {accountId}");
        }
        return Ok(reserveringen);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservering>>> GetAlleReserveringenPerVoertuig(int voertuigId)
    {
        var voertuig = await _context.Voertuigen.FindAsync(voertuigId);
        if (voertuig == null) return NotFound();
        voertuig.GetReserveringen();
        return Ok(voertuig);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<Reservering>>> PostReservering(Reservering reservering)
    {
        if (reservering == null) return BadRequest();
        
        var account = await _context.Accounts.OfType<AccountZakelijk>()
            .FirstOrDefaultAsync(a => a.AccountId == reservering.Account.AccountId);
        
        if (account == null) return NotFound();
        
        account.AddReservering(reservering);
        _context.Reserveringen.Add(reservering);
        
        for (int i = 0; reservering.GereserveerdeVoertuigen.Count() > i; i++)
        {
            var voertuig = reservering.GereserveerdeVoertuigen.ElementAt(i);
            voertuig.Reserveringen.Add(reservering);
        }
        
        await _context.SaveChangesAsync();
        
        return Ok(reservering);
    }
}