using Microsoft.EntityFrameworkCore;
using WishList.Storage.Entities;

namespace WishList.Storage;

public class WishListDbContext(DbContextOptions<WishListDbContext> options) : DbContext(options)
{
    public DbSet<TelegramUser> Users { get; init; }
    public DbSet<Entities.WishList> WishLists { get; init; }
    public DbSet<Present> Presents { get; init; }
}