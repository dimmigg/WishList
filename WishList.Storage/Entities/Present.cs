using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Storage.Entities;

public class Present
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; init; }
    
    [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
    public string Comment { get; set; }

    [StringLength(500, ErrorMessage = "Reference cannot be longer than 500 characters.")]
    public string Reference { get; set; }
    
    public int WishListId { get; init; }
    
    [ForeignKey(nameof(WishListId))]
    public WishList WishList { get; set; }
    public long? ReserveForUserId { get; set; }
}