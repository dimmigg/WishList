using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Storage.Entities;

public class Present
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public string? Comment { get; set; }

    public string? Reference { get; set; }
    
    public int WishListId { get; init; }
    
    [ForeignKey(nameof(WishListId))]
    public WishList WishList { get; set; }
    public long? ReserveForUserId { get; set; }
}