using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Marka> Marke { get; set; }
    public DbSet<Klima> Klime { get; set; }
}
