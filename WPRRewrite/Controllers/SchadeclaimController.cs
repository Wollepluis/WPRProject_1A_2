﻿    using Microsoft.AspNetCore.Identity;
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
        
        [HttpGet("krijgalleSchadeclaims")]
        public async Task<ActionResult<IEnumerable<Schadeclaim>>> GetAlleSchadeclaims()
        {
            var schadeclaims = await _context.Schadeclaim
                .OrderBy(a => a.Schadeclaimstatus == "Afgehandeld") // Eerst niet-afgehandeld, dan afgehandeld
               
                .ToListAsync();

            return Ok(schadeclaims);
        }
        
        [HttpPost("PostSchadeclaim")]
        public async Task<ActionResult<IEnumerable<Schadeclaim>>> PostSchadeclaim(SchadeclaimDto schadeclaimDto)
        {
            if (schadeclaimDto == null) return BadRequest();
            
            if (!(_context.Voertuigen.Any(a => a.VoertuigId == schadeclaimDto.VoertuigId)))
                return NotFound("Geen voertuig gevonden");
            
            var voertuig = await _context.Voertuigen.FindAsync(schadeclaimDto.VoertuigId);
            if (voertuig == null) return BadRequest("Voertuig niet gevonden");

            voertuig.VoertuigStatus = "Geblokkeerd";
            
            Schadeclaim schadeclaim = new Schadeclaim(schadeclaimDto.Beschrijving, schadeclaimDto.VoertuigId, schadeclaimDto.Datum);
        
                _context.Schadeclaim.Add(schadeclaim);
        
            await _context.SaveChangesAsync();
        
            return Ok(schadeclaim);
        }

        [HttpPut("UpdateStatus")]
        public async Task<ActionResult<Schadeclaim>> UpdateStatus(int schadeclaimId, string status)
        {
            var schadeclaim = await _context.Schadeclaim.FindAsync(schadeclaimId);
            if (schadeclaim == null) return NotFound("Schadeclaim niet gevonden");
            
            schadeclaim.Schadeclaimstatus = status;
            await _context.SaveChangesAsync();
            return NoContent();
        }
}