using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Storage.Entities;

public class Present
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string? Comment { get; set; }

    public string? Reference { get; set; }
    
    public int WishListId { get; set; }
    
    [ForeignKey(nameof(WishListId))]
    public WishList WishList { get; set; }
}