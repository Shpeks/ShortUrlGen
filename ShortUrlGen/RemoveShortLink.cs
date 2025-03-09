using Quartz;
using ShortUrlGen.Data;

namespace ShortUrlGen
{
    public class RemoveShortLinkJob : IJob
    {
        private readonly ApplicationDbContext _context;
        public RemoveShortLinkJob(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var expiredLinks = _context.UrlMappings.Where(u => u.ExpiresAt < DateTime.Now).ToList();
            int count = 0;

            foreach (var item in expiredLinks)
            {
                _context.UrlMappings.Remove(item);
                count++;
            }

            await _context.SaveChangesAsync();

            Console.WriteLine($"URL removed {DateTime.Now}, Count = {count}");
        }
    }
}
