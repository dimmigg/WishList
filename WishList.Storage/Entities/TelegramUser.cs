using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Storage.Entities;

public class TelegramUser
{
    [Key]
    public long Id { get; set; }
    
    public string FirstName { get; set; } = default!;
    
    public string? LastName { get; set; }
    
    public string? Username { get; set; }
    
    public string? LastCommand  { get; set; }
    
    [InverseProperty(nameof(WishList.Author))]
    public ICollection<WishList> WishLists { get; set; }
    
    [InverseProperty(nameof(WishList.ReadUsers))]
    public ICollection<WishList> SubscribeWishLists { get; set; }
    
    [InverseProperty(nameof(WishList.WriteUsers))]
    public ICollection<WishList> WriteWishLists { get; set; }
    
    public override string ToString() =>
        $"{(Username is null ? $"{FirstName}{LastName?.Insert(0, " ")}" : $"@{Username}")}";
}