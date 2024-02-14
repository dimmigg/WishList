namespace WishList.Domain.Models;

public class WishList
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public long AuthorId { get; set; }

    public RegisteredUser Author { get; set; }

    public bool IsPrivate { get; set; }

    public ICollection<RegisteredUser> ReadUsers { get; set; }
    
    public ICollection<RegisteredUser> WriteUsers { get; set; }
}