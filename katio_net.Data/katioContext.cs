using Microsoft.EntityFrameworkCore;
using katio.Data.Models;


namespace katio.Data;

public class KatioContext : DbContext
{
    public KatioContext(DbContextOptions<KatioContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null;
    public DbSet<Author> Authors { get; set; } = null;
    public DbSet<Narrator> Narrators { get; set; } = null;
    public DbSet<Genre> Genres { get; set; } = null;
    public DbSet<AudioBook> AudioBooks { get; set; } = null;



    protected override void OnModelCreating(ModelBuilder builder)
    {
        if(builder == null)
        {
            return;
        }

        builder.Entity<Book>().ToTable("Book").HasKey(k => k.Id);
        builder.Entity<Author>().ToTable("Author").HasKey(k => k.Id);
        builder.Entity<Narrator>().ToTable("Narrator").HasKey(k => k.Id);
        builder.Entity<Genre>().ToTable("Genre").HasKey(k => k.Id);
        builder.Entity<AudioBook>().ToTable("AudioBook").HasKey(k => k.Id);
        base.OnModelCreating(builder);
    }
}
