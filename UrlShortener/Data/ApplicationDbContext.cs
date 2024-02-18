using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<UrlManager> Urls { get; set; }
    }
}
