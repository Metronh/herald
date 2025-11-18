using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UploadData.AppSettings;
using UploadData.Models;

namespace UploadData.Repository.Context;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}