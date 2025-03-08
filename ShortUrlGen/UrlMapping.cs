using System.ComponentModel.DataAnnotations;

namespace ShortUrlGen
{
    public class UrlMapping
    {
        [Key]
        public int Id { get; set; }
        
        public string LongUrl {  get; set; } // Изначальная ссылка

        public string ShortUrl { get; set; }

        public DateTime CreateAt { get; set; } 

        public DateTime? UpdateAt { get; set; } // Последнее обновление

        public DateTime? ExpiresAt { get; set; } // Время истечения в секундах

        public int Count { get; set; } // Количество переходов
    }
}
