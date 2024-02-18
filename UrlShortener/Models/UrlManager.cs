using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models {
    public class UrlManager {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = "";

        [DisplayName("Short Url")]
        public string ShortUrl { get; set; } = "";
    }
}
