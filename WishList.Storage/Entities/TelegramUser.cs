﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Storage.Entities;

public class TelegramUser
{
    [Key]
    public long Id { get; init; }
    
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string FirstName { get; set; } = default!;
    
    [StringLength(500, ErrorMessage = "LastName cannot be longer than 500 characters.")]
    public string? LastName { get; set; }
    
    [StringLength(500, ErrorMessage = "Username cannot be longer than 500 characters.")]
    public string? Username { get; set; }
    
    [StringLength(100, ErrorMessage = "LastCommand cannot be longer than 100 characters.")]
    public string? LastCommand  { get; set; }
    
    [InverseProperty(nameof(WishList.Author))]
    public ICollection<WishList> WishLists { get; set; }
    
    [InverseProperty(nameof(WishList.ReadUsers))]
    public ICollection<WishList> SubscribeWishLists { get; set; }
    
    [InverseProperty(nameof(WishList.WriteUsers))]
    public ICollection<WishList> WriteWishLists { get; set; }
    
    public override string ToString() =>
        $"{(Username is null ? $"{FirstName}{LastName?.Insert(0, " ")}" : $"@{Username}")}";
    
    public override bool Equals(object? obj)
    {
        if (obj is not TelegramUser otherUser)
            return false;

        return Id == otherUser.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(TelegramUser user1, TelegramUser user2)
    {
        // Если оба объекта null, то они равны
        if (ReferenceEquals(user1, null) && ReferenceEquals(user2, null))
            return true;
        
        // Если один из объектов null, то они не равны
        if (ReferenceEquals(user1, null) || ReferenceEquals(user2, null))
            return false;
        
        return user1.Equals(user2);
    }

    public static bool operator !=(TelegramUser user1, TelegramUser user2)
    {
        return !(user1 == user2);
    }
}