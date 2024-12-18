﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<BirthDayController> _logger;

        public BirthDayController(
            DataContext context,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BirthDayController> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("hide-guests-get-chek-for")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Guests()
        {
            var res = await _context.Guest.ToListAsync();
            res.ForEach(x => x.Name = x.Name.Trim());
            return Ok(res);
        }

        [HttpGet("check-guest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Guest?> CheckGues(string ident)
        {
            var res = await _context.Guest
                .Where(x => x.Key.Trim().ToLower() == ident.ToLower())
                .FirstOrDefaultAsync();

            if (res != null)
            {
                res.Name = res.Name.Trim();

                return new()
                {
                    Name = res.Name,
                    Answer = res.Answer,
                };
            }

            return null;
        }

        [HttpPut("guest-say-yes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> GuestSayYes(string ident)
        {
            var res = await _context.Guest
                .Where(x => x.Key.Trim().ToLower() == ident.ToLower())
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
                .Where(x => x.Key.Trim().ToLower() == ident.ToLower())
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
            var result = answer ? "'приду'" : "'не приду'";
            var message = $"{guest.Name.Trim()} сказал {result}";

            message += "\nвсе:\n";
            message += "https://www.vika-birthday.ru/api/birth-day/hide-guests-get-chek-for";

            await SendToChat("1077072257", message);
            await SendToChat("1338551358", message);
        }

        private async Task SendToChat(string chanel, string message)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var key = _configuration["TelegramBotApi:ApiKey"];
                var headreKey = _configuration["TelegramBotApi:ApiKeyHeader"];

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"http://custplace.ru/education-bot/telegram/send-message?chanel={chanel}&message={message}");

                request.Headers.Add(headreKey, key);

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var responseTxt = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"IsSuccessStatusCode: {responseTxt}");
                    _logger.LogError($"ExceIsSuccessStatusCodeption: {responseTxt}");
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                _logger.LogError($"Exception: {ex.Message}", ex);
            }
        }

    }
}
