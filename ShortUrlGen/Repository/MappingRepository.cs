using Base62;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ShortUrlGen.Data;
using ShortUrlGen.Data.Models;
using ShortUrlGen.Interfaces;
using System.Buffers.Text;
using System.Xml.Serialization;

namespace ShortUrlGen.Repository
{
    public class MappingRepository : IMappingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Base62Converter _base62;
        public MappingRepository(ApplicationDbContext context, Base62Converter base62)
        {
            _context = context;
            _base62 = base62;
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
            Uri uri = new Uri(longUrl);
            string path = uri.AbsolutePath;

            var shortUrl = _base62.Encode(path);

            if (shortUrl.Length > 10)
            {
                shortUrl = shortUrl.Substring(0, 10);
            }

            return shortUrl;
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
