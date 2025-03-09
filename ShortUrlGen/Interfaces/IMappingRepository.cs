using ShortUrlGen.Data.Models;

namespace ShortUrlGen.Interfaces
{
    public interface IMappingRepository
    {
        UrlMapping SaveUrlMapping(string longUrl, int second, string shortUrl);
        string ShortUrlGenerate(string longUrl);
        void UrlMappingUpdate(UrlMapping urlMapping);
        UrlMapping GetLongUrlByShortUrl(string shortUrl);
    }
}