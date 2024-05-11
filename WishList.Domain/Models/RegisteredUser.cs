namespace WishList.Domain.Models;

public class RegisteredUser
{
    public long Id { get; init; }
    
    public string FirstName { get; set; } = default!;
    
    public string? LastName { get; set; }
    
    public string? Username { get; set; }
    
    public string LastCommand  { get; set; }
    
    public ICollection<WishList> WishLists { get; set; }
    
    public ICollection<WishList> SubscribeWishLists { get; set; }
    
    public ICollection<WishList> WriteWishLists { get; set; }
    
    public override string ToString() =>
        $"{(Username is null ? $"{FirstName}{LastName?.Insert(0, " ")}" : $"@{Username}")}";
    
    public override bool Equals(object? obj)
    {
        if (obj is not RegisteredUser)
            return false;
        
        var otherUser = (RegisteredUser)obj;
        return Id == otherUser.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(RegisteredUser user1, RegisteredUser user2)
    {
        // Если оба объекта null, то они равны
        if (ReferenceEquals(user1, null) && ReferenceEquals(user2, null))
            return true;
        
        // Если один из объектов null, то они не равны
        if (ReferenceEquals(user1, null) || ReferenceEquals(user2, null))
            return false;
        
        return user1.Equals(user2);
    }

    public static bool operator !=(RegisteredUser user1, RegisteredUser user2)
    {
        return !(user1 == user2);
    }
}