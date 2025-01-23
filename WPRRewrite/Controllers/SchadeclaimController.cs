    using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
    [Route("api/Schadeclaim")]
public class SchadeclaimController : ControllerBase
{
        private readonly CarAndAllContext _context;
        private readonly IPasswordHasher<Account> _passwordHasher;
    
        public SchadeclaimController(CarAndAllContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        [HttpPost("PostSchadeclaim")]
        public async Task<ActionResult<IEnumerable<Reservering>>> PostReservering(SchadeclaimDto schadeclaimDto)
        {
            if (schadeclaimDto == null) return BadRequest();
            
            if (!(_context.Voertuigen.Any(a => a.VoertuigId == schadeclaimDto.VoertuigId)))
                return BadRequest("Geen voertuig gevonden");

            Schadeclaim schadeclaim = new Schadeclaim(schadeclaimDto.Beschrijving, schadeclaimDto.VoertuigId, schadeclaimDto.Datum);
        
                _context.Schadeclaim.Add(schadeclaim);
        
            await _context.SaveChangesAsync();
        
            return Ok(schadeclaim);
        }
}