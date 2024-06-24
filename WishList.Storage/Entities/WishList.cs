using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Storage.Entities;

public class WishList
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [StringLength(500, ErrorMessage = "Name cannot be longer than 500 characters.")]
    public string Name { get; set; }
    
    public long AuthorId { get; init; }

    [ForeignKey(nameof(AuthorId))]
    [Required]
    [StringLength(500, ErrorMessage = "Author cannot be longer than 500 characters.")]
    public TelegramUser Author { get; set; }

    public bool IsPrivate { get; set; }

    [InverseProperty(nameof(TelegramUser.SubscribeWishLists))]
    public ICollection<TelegramUser> ReadUsers { get; set; }
    
    [InverseProperty(nameof(TelegramUser.WriteWishLists))]
    public ICollection<TelegramUser> WriteUsers { get; set; }
    
    [InverseProperty(nameof(Present.WishList))]
    public ICollection<Present> Presents { get; set; }
    
}