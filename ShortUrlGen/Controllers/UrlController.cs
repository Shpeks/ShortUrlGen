using System.Xml.Serialization;
using Base62;
using Microsoft.AspNetCore.Mvc;
using ShortUrlGen.Interfaces;

namespace ShortUrlGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IMappingRepository _mappingRepository;
        public UrlController(IMappingRepository mappingRepository)
        {
            _mappingRepository = mappingRepository;   
        }

        [HttpPost]
        public IActionResult CreateShortUrl(string longUrl, int second)
        {
            try
            {
                var shortUrl = _mappingRepository.ShortUrlGenerate(longUrl);
                var mapping = _mappingRepository.SaveUrlMapping(longUrl, second, shortUrl);

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
                var urlMapping = _mappingRepository.GetLongUrlByShortUrl(shortUrl);

                if (urlMapping == null)
                {
                    return NotFound("Not found");
                }

                _mappingRepository.UrlMappingUpdate(urlMapping);

                return Ok(urlMapping.LongUrl);
            }
            catch (Exception)
            {
                return StatusCode(500, "Problem from server");
            }
        }
    }
}
