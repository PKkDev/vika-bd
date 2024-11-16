using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VikaBD.Server.Context;
using VikaBD.Server.Model;

namespace VikaBD.Server.Controllers
{
    [Route("birth-day")]
    [ApiController]
    public class BirthDayController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<BirthDayController> _logger;

        public BirthDayController(
            ILogger<BirthDayController> logger,
            DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("guests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Guests()
        {
            var res = await _context.Guest.ToListAsync();
            res.ForEach(x => x.Name = x.Name.Trim());
            return Ok(res);
        }

        [HttpGet("check-guest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Guest> CheckGues(string ident)
        {
            var res = await _context.Guest
                .Where(x => x.Name.Trim().ToLower() == ident.ToLower() || x.Id.ToString() == ident)
                .FirstOrDefaultAsync();

            if (res != null)
            {
                res.Name = res.Name.Trim();
            }

            return res;
        }

        [HttpPut("guest-say-yes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> GuestSayYes(string ident)
        {
            var res = await _context.Guest
                .Where(x => x.Name == ident || x.Id.ToString() == ident)
                .FirstOrDefaultAsync();

            if (res != null)
            {
                res.Answer = true;

                _context.Guest.Update(res);
                await _context.SaveChangesAsync();
            }

            return res != null;
        }

        [HttpPut("guest-say-no")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> GuestSayNo(string ident)
        {
            var res = await _context.Guest
                .Where(x => x.Name == ident || x.Id.ToString() == ident)
                .FirstOrDefaultAsync();

            if (res != null)
            {
                res.Answer = false;

                _context.Guest.Update(res);
                await _context.SaveChangesAsync();
            }

            return res != null;
        }

    }
}
