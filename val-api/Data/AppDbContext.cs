using Microsoft.EntityFrameworkCore;

namespace RandomQuoteApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        mb.Entity<Category>().HasData(
            new Category { Id = 1, Name = "sweet" },
            new Category { Id = 2, Name = "funny" },
            new Category { Id = 3, Name = "dark" },
            new Category { Id = 4, Name = "sarcastic" }
        );
    }
}