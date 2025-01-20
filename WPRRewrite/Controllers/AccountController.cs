using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Enums;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("[Controller]")]
public class AccountController(Context context) : ControllerBase
{
    private readonly Context _context = context ?? throw new ArgumentNullException(nameof(context));

    [HttpGet("GetAccounts")]
    public async Task<ActionResult<IEnumerable<IAccount>>> GetAccounts([FromQuery] AccountTypeEnum? accountType, 
        [FromQuery] int? accountId)
    {
        try
        {
            IQueryable<IAccount> query = _context.Accounts;
            
            if (accountId.HasValue)
            {
                var account = await query.FirstOrDefaultAsync(a => a.AccountId == accountId);
                if (account == null)
                    return NotFound(new { Message = $"Account met ID {accountId} niet gevonden" });
                
                return Ok(account);
            }

            if (accountType.HasValue)
            {
                query = accountType switch
                {
                    AccountTypeEnum.Particulier => query.OfType<AccountParticulier>(),
                    AccountTypeEnum.ZakelijkBeheerder => query.OfType<AccountZakelijkBeheerder>(),
                    AccountTypeEnum.ZakelijkHuurder => query.OfType<AccountZakelijkHuurder>(),
                    AccountTypeEnum.Frontoffice => query.OfType<AccountMedewerkerFrontoffice>(),
                    AccountTypeEnum.Backoffice => query.OfType<AccountMedewerkerBackoffice>(),
                    _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, "Onjuist account type")
                };
            }

            var accounts = await query.ToListAsync();
            if (accounts.Count == 0)
                return NotFound(new { Message = "Geen accounts met dit type gevonden" });

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        try
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == login.Email);
            if (account == null) 
                return Unauthorized(new { Message = $"Account {login.Email} niet gevonden" });
        
            if (account.VerifieerWachtwoord(login.Wachtwoord) == PasswordVerificationResult.Failed) 
                return Unauthorized(new { Message = "Incorrect wachtwoord" });
            
            var reservering = await _context.Reserveringen
                .FirstOrDefaultAsync(r => r.AccountId == account.AccountId);
            if (reservering == null) 
                return Ok(account);
            
            var reserveringDate = reservering.Begindatum;
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (reserveringDate != currentDate.AddDays(1) || reservering.Herinnering) 
                return Ok(account);
            
            EmailSender.VerstuurHerinneringEmail(account.Email, reservering.VoertuigId, reservering.Begindatum);
            
            reservering.UpdateHerinnering();
            await _context.SaveChangesAsync();

            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
    [HttpPost("Registreer")]
    public async Task<ActionResult<IAccount>> Create([FromBody] AccountDto gegevens)
    {
        try
        {
            var checkEmail = _context.Accounts.Any(a => a.Email == gegevens.Email);
            if (checkEmail) 
                return BadRequest(new { Message = "Een gebruiker met deze Email bestaat al" });
        
            var nieuwAccount = Account.MaakAccount(gegevens);
    
            _context.Accounts.Add(nieuwAccount);
            await _context.SaveChangesAsync();
        
            EmailSender.VerstuurBevestigingEmail(nieuwAccount.Email);
        
            return Ok(new { nieuwAccount.Email });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromQuery] int id, [FromBody] AccountDto nieuweGegevens)
    {
        try
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound(new { Message = $"Account met ID {id} staat niet in de database" });
        
            // Update Adres voor Particulier account????
            account.UpdateAccount(nieuweGegevens);

            return Ok(new { Message = "Account succesvol aangepast" }); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] int accountId)
    {
        try
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) 
                return NotFound(new { Message = $"Account met ID {accountId} staat niet in de database" });

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        
            EmailSender.VerstuurVerwijderEmail(account.Email);

            return Ok(new { Message = $"Account {account.Email} succesvol verwijderd" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
    //## Frontoffice Functies ##
    [HttpPut("UpdateVoertuigStatus")]
    public async Task<IActionResult> PutVoertuigStatus([FromQuery] int id, [FromQuery] DateOnly begindatum, 
        [FromQuery] DateOnly einddatum)
    {
        try {
            var voertuig = await _context.Voertuigen.FindAsync(id);
            if (voertuig == null) return NotFound();

            switch (voertuig.VoertuigStatus)
            {
                case VoertuigStatusEnum.Gereserveerd:
                case VoertuigStatusEnum.Beschikbaar:
                    voertuig.VoertuigStatus = VoertuigStatusEnum.Uitgegeven;
                    break;
                case VoertuigStatusEnum.Uitgegeven:
                    voertuig.VoertuigStatus = VoertuigStatusEnum.Beschikbaar;
                    break;
                default:
                    return BadRequest("Ongeldige VoertuigStatus");
            }
        
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }
    
    //## Backoffice Functies ##
    [HttpPut("HuuraanvraagKeuren")]
    public async Task<ActionResult> HuuraanvraagKeuren([FromBody] HuuraanvraagDto huuraanvraagDto)
    {
        try
        {
            var reservering = await _context.Reserveringen
                .FirstOrDefaultAsync(a => a.ReserveringId == huuraanvraagDto.ReserveringId);
            if (reservering == null)
                return NotFound(new { Message = "Huuraanvraag niet gevonden." });
            
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == reservering.AccountId);
            
            var voertuig = await _context.Voertuigen
                .FirstOrDefaultAsync(a => a.VoertuigId == reservering.VoertuigId);
            reservering.IsGoedgekeurd = huuraanvraagDto.Keuze;
            reservering.Comment = huuraanvraagDto.Comment?? reservering.Comment;
            
            if (!reservering.IsGoedgekeurd)
            {
                EmailSender.AanvraagAfgekeurd(account.Email, reservering.Begindatum, reservering.Einddatum, 
                    voertuig.Merk, voertuig.Model, voertuig.VoertuigType.ToString(), reservering.Comment);
                _context.Reserveringen.Remove(reservering);
            }
            else
            {
                EmailSender.AanvraagGoedgekeurd(account.Email, reservering.Begindatum, reservering.Einddatum, 
                    voertuig.Merk, voertuig.Model, voertuig.VoertuigType.ToString(), reservering.Comment);
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Huuraanvraag is bijgewerkt." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }
}