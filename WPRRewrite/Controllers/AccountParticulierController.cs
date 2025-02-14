﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/Particulier")]
public class AccountParticulierController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IAdresService _adresService;

    public AccountParticulierController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher, IAdresService adresService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _adresService = adresService ?? throw new ArgumentNullException(nameof(adresService));
    }
    
    [HttpGet("KrijgAlleAccounts")]
    public async Task<ActionResult<IEnumerable<AccountParticulier>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountParticulier>().ToListAsync();
    }
    
    [HttpGet("KrijgAccountnaam")]
    public async Task<ActionResult<string>> GetAccountnaam(int accountId)
    {
        var account =  await _context.Accounts.OfType<AccountParticulier>()
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        return account.Naam;

    }


    
    
    
    
    [HttpGet("KrijgSpecifiekAccount")]
    public async Task<ActionResult<AccountParticulier>> GetAccount(int id)
    {
        var account = await _context.Accounts.OfType<AccountParticulier>().Include(a => a.Adres).Where(a => a.AccountId == id).FirstOrDefaultAsync();
        account.AccountType = "Particulier";
        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }
    
    
    
    
    
    
    [HttpGet("KrijgSpecifiekAccountEmail")]
    public async Task<ActionResult<AccountParticulier>> GetAccountEmail(string email)
    {
        var account = await _context.Accounts.OfType<AccountParticulier>().Where(a => a.Email == email).FirstOrDefaultAsync();

        if (account == null) return NotFound();
        return Ok(account);
    }
    
    
    
    
    
    
    [HttpPost("MaakAccount")]
    public async Task<ActionResult<AccountParticulier>> PostAccount([FromBody] ParticulierDto accountDto)
    {
        var anyEmail = _context.Accounts.Any(a => a.Email == accountDto.Email);
        
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        if (accountDto == null) return BadRequest("Accountgegevens mogen niet leeg zijn.");
        
        var adres = await _context.Adressen.Where(a => a.Huisnummer == accountDto.Huisnummer && a.Postcode == accountDto.Postcode).FirstOrDefaultAsync();
        if (adres == null)
        {
            adres = await _adresService.ZoekAdresAsync(accountDto.Postcode, accountDto.Huisnummer);
            
            if (adres == null) return NotFound("Het adres is niet gevonden met de bijbehorende postcode en huisnummer...");
        
            _context.Adressen.Add(adres);
            await _context.SaveChangesAsync();
        }
        await _context.SaveChangesAsync();
                
        AccountParticulier account = new AccountParticulier(accountDto.Email, accountDto.Wachtwoord, accountDto.Naam, adres.AdresId, accountDto.Telefoonnummer, _passwordHasher, _context);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);
        
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        try { EmailSender.VerstuurBevestigingsEmail(account.Email); } catch {  }
        return Ok(new { AccountId = account.AccountId});
    }
    
    
    
    
    
    
    [HttpPost("Login")]
         public async Task<IActionResult> Login([FromBody] LoginDto accountDto)
         {
             var account = await _context.Accounts.OfType<AccountParticulier>().FirstOrDefaultAsync(a => a.Email == accountDto.Email);
             if (account == null) return Unauthorized(new { message = "Account is niet gevonden"});

             var result = _passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord);

             if (result == PasswordVerificationResult.Failed) return Unauthorized(new { message = "Verkeerd wachtwoord"});

             var reservering = await _context.Reserveringen.FirstOrDefaultAsync(r => r.AccountId == account.AccountId);
             
             if (reservering != null)
             {
                 var currentDate = DateTime.Now.Date;
            
                 if (reservering.Begindatum.Date == currentDate.AddDays(1))
                 {
                     EmailSender.VerstuurHerinneringEmail(account.Email, reservering.VoertuigId, reservering.Begindatum);
                     //reservering.UpdateHerinnering();
                     await _context.SaveChangesAsync();
                 }
                 else if (reservering.Einddatum.Date == currentDate.AddDays(-1))
                 {
                     EmailSender.VerstuurHerinneringEmail2(account.Email, reservering.VoertuigId, reservering.Einddatum);
                     //reservering.UpdateHerinnering();
                     await _context.SaveChangesAsync();
                 }
             }
             
             return Ok(new {account.AccountId});
         }

         
         
         
         
         
    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> PutAccount([FromQuery]int id, [FromBody]ParticulierDto accountDto)
    {
        var existingAccount = await _context.Accounts.OfType<AccountParticulier>().FirstOrDefaultAsync(a => a.AccountId == id);
        if (existingAccount == null) return NotFound();
        
        var nieuwAdres = await _context.Adressen.Where(a => a.Huisnummer == accountDto.Huisnummer && a.Postcode == accountDto.Postcode).FirstOrDefaultAsync();
        if (nieuwAdres == null)
        {
            try
            {
                nieuwAdres = await _adresService.ZoekAdresAsync(accountDto.Postcode, accountDto.Huisnummer);
            }
            catch (Exception e)
            {
                return NotFound("Het adres is niet gevonden met de bijbehorende postcode en huisnummer...");
            }
            if (nieuwAdres == null) return NotFound("Address niet gevonden voor de gegeven postcode en huisnummer.");
            _context.Adressen.Add(nieuwAdres);
            
            await _context.SaveChangesAsync();
        }
        var accounts = _context.Accounts.OfType<AccountParticulier>().Count(a => a.AdresId == existingAccount.AdresId);
        var bedrijven = _context.Bedrijven.Count(a => a.AdresId == existingAccount.AdresId);
        if ((accounts + bedrijven) == 1)
        {
            Adres? oudAdres = await _context.Adressen.FirstOrDefaultAsync();
            if (oudAdres == null) return NotFound("Adres niet gevonden");
            _context.Adressen.Remove(oudAdres);
        }
        
        AccountParticulier account = new AccountParticulier(existingAccount.Email, accountDto.Wachtwoord, accountDto.Naam, nieuwAdres.AdresId, accountDto.Telefoonnummer, _passwordHasher, _context);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);
        existingAccount.UpdateAccount(account);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    
    
    
    
    
    [HttpDelete("VerwijderParticulier")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    
}