using LyricDb.Contracts.Models;
using LyricDb.Web.Models.Dao;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LyricDb.Web;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    
    public DbSet<Song> Songs { get; init; }
    public DbSet<Message> Messages { get; init; }
    public DbSet<Lyric> Lyrics { get; init; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Song>()
            .Navigation(t => t.Submitter).AutoInclude();
        builder.Entity<Lyric>()
            .Navigation(t => t.Song).AutoInclude();
        builder.Entity<Lyric>()
            .Navigation(t => t.Submitter).AutoInclude();
        base.OnModelCreating(builder);
    }
}