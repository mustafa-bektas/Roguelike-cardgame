using UnityEngine;

// First, define an enum for suits:
public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

// If you like, define an enum for ranks, OR we can use int for rankValue
// This is optional, but let's show you both approaches:

public enum Rank
{
    Two    = 2,
    Three  = 3,
    Four   = 4,
    Five   = 5,
    Six    = 6,
    Seven  = 7,
    Eight  = 8,
    Nine   = 9,
    Ten    = 10,
    Jack   = 11,
    Queen  = 12,
    King   = 13,
    Ace    = 14
}

// Now define the main CardData class:
[System.Serializable]
public class CardData
{
    public Suit suit;        // e.g. Suit.Clubs
    public Rank rank;        // e.g. Rank.Two
    
    // We'll also store a Sprite reference so we can show the card's image
    public Sprite cardSprite;
    
    // Optionally, store the prefab if needed
    // public GameObject cardPrefab;
    
    // A helper property to combine suit & rank into a readable name
    public string CardName => suit + "_" + rank; // e.g. "Clubs_Two"
}