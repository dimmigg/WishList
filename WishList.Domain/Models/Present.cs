namespace WishList.Domain.Models;

public class Present
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string Comment { get; set; }

    public string Reference { get; set; }
    
    public int WishListId { get; set; }
    
    public WishList WishList { get; set; }
}