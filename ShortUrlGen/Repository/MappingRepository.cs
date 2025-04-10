using System.Security.Cryptography;
using ShortUrlGen.Data;
using ShortUrlGen.Data.Models;
using ShortUrlGen.Interfaces;
using MhanoHarkness;

namespace ShortUrlGen.Repository
{
    public class MappingRepository : IMappingRepository
    {
        private readonly ApplicationDbContext _context;
        public MappingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UrlMappingUpdate(UrlMapping urlMapping)
        {
            urlMapping.Count++;
            urlMapping.UpdateAt = DateTime.Now;
            urlMapping.ExpiresAt = DateTime.Now.AddSeconds(10);

            _context.SaveChanges();
        }

        public UrlMapping GetLongUrlByShortUrl(string shortUrl)
        {
            var longUrl = _context.UrlMappings.FirstOrDefault(d => d.ShortUrl == shortUrl);

            return longUrl;
        }

        public string ShortUrlGenerate(string longUrl)
        {
            var rng = RandomNumberGenerator.Create();
            var unit32Buffer = new byte[8];
            rng.GetBytes(unit32Buffer);

            var shortUrl = Base32Url.ToBase32String(unit32Buffer);

            return shortUrl.ToString();
        }

        public UrlMapping SaveUrlMapping(string longUrl, int second, string shortUrl)
        {
            var getUrl = _context.UrlMappings.FirstOrDefault(lu => lu.LongUrl == longUrl);

            if (getUrl != null)
            {
                return getUrl;
            }
            else
            {
                var mapping = new UrlMapping
                {
                    LongUrl = longUrl,
                    Count = 1,
                    ShortUrl = shortUrl,
                    CreateAt = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy H:m:ss")),
                    ExpiresAt = DateTime.Now.AddSeconds(second),
                };

                _context.UrlMappings.Add(mapping);
                _context.SaveChanges();

                return mapping;
            }
        }
    }
}
