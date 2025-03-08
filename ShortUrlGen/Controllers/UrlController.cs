using System.Xml.Serialization;
using Base62;
using Microsoft.AspNetCore.Mvc;

namespace ShortUrlGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Base62Converter _base62;
        public UrlController(ApplicationDbContext context)
        {
            _context = context;
            _base62 = new Base62Converter();
        }

        [HttpPost]
        public IActionResult CreateShortUrl(string longUrl, int second)
        {
            try
            {
                Uri uri = new Uri(longUrl);
                string path = uri.AbsolutePath;

                var shortUrl = _base62.Encode(path);

                if (shortUrl.Length > 10)
                {
                    shortUrl.Substring(0, 10);
                }

                var mapping = new UrlMapping
                {
                    LongUrl = longUrl,
                    Count = 1,
                    ShortUrl = shortUrl,
                    CreateAt = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy H:m")),
                    ExpiresAt = DateTime.Now.AddSeconds(second),
                };

                _context.UrlMappings.Add(mapping);
                _context.SaveChanges();

                return Ok(new { shortUrl = $"http://localhost:5051/{mapping.ShortUrl}" });
            }
            catch (UriFormatException)
            {
                return BadRequest("Uncorrect format URL.");
            }
            catch (Exception) 
            { 
                return StatusCode(500, "Problem from server.");
            }
        }

        [HttpGet]
        public IActionResult RedirectToLongUrl(string shortUrl)
        {
            try
            {
                var urlMapping = _context.UrlMappings.FirstOrDefault(d => d.ShortUrl == shortUrl);

                if (urlMapping == null)
                {
                    return NotFound("Not found");
                }

                urlMapping.Count++;
                urlMapping.UpdateAt = DateTime.Now;
                urlMapping.ExpiresAt = DateTime.Now.AddSeconds(10);

                _context.SaveChanges();

                return Ok(urlMapping.LongUrl);
            }
            catch (Exception)
            {
                return StatusCode(500, "Problem from server");
            }
        }
    }
}
