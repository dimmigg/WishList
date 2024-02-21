using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Storage.Entities;

public class WishList
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    
    public long AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    [Required]
    public TelegramUser Author { get; set; }

    public bool IsPrivate { get; set; }

    [InverseProperty(nameof(TelegramUser.SubscribeWishLists))]
    public ICollection<TelegramUser> ReadUsers { get; set; }
    
    [InverseProperty(nameof(TelegramUser.WriteWishLists))]
    public ICollection<TelegramUser> WriteUsers { get; set; }
    
    [InverseProperty(nameof(Present.WishList))]
    public ICollection<Present> Presents { get; set; }
    
}