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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BirthDayController(
            DataContext context,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
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

                await SendAnswer(res, true);
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

                await SendAnswer(res, false);
            }

            return res != null;
        }

        private async Task SendAnswer(Guest guest, bool answer)
        {
            try
            {
                var result = answer ? "'приду'" : "'не приду'";
                var message = $"{guest.Name.Trim()} сказал {result}";

                message += "\nвсе:\n";
                message += "https://www.vika-birthday.ru/api/birth-day/guests";

                await SendToChat("1077072257", message);
                await SendToChat("1338551358", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task SendToChat(string chanel, string message)
        {
            var client = _httpClientFactory.CreateClient();

            var key = _configuration["TelegramBotApi:ApiKey"];
            var headreKey = _configuration["TelegramBotApi:ApiKeyHeader"];

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://custplace.ru/education-bot/telegram/send-message?chanel={chanel}&message={message}");

            request.Headers.Add(headreKey, key);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

    }
}
