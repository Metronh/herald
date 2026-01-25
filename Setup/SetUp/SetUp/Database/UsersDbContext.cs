using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SetUp.AppSettings;
using SetUp.Models;

namespace SetUp.Database;

public class UsersDbContext : DbContext
{
    private readonly ConnectionStrings _connectionStrings;
    public UsersDbContext(DbContextOptions<UsersDbContext> options, IOptions<ConnectionStrings> connectionStrings) : base(options)
    {
        _connectionStrings = connectionStrings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionStrings.PostgresDb);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
    }

    public DbSet<User> Users { get; set; }
}