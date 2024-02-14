﻿namespace WishList.Domain.Models;

public class RegisteredUser
{
    public long Id { get; set; }
    
    public string FirstName { get; set; } = default!;
    
    public string? LastName { get; set; }
    
    public string? Username { get; set; }
    
    public int? CurrentWay { get; set; }
    
    public int? WayStep { get; set; }
    
    public ICollection<WishList> WishLists { get; set; }
    
    public ICollection<WishList> ReadWishLists { get; set; }
    
    public ICollection<WishList> WriteWishLists { get; set; }
    
    public override string ToString() =>
        $"{(Username is null ? $"{FirstName}{LastName?.Insert(0, " ")}" : $"@{Username}")} ({Id})";
}