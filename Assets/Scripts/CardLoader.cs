using UnityEngine;
using System.Collections.Generic;

public class CardLoader : MonoBehaviour
{
    // We'll store all CardData in a list for easy reference
    public List<CardData> allCards = new List<CardData>();

    // Specify the path inside "Resources" where your prefabs live
    [SerializeField] private string resourcesFolderPath = "Cards";
    [SerializeField] private DeckManager deckManager;

    private void Awake()
    {
        // 1. Load all prefabs from Resources/Cards
        GameObject[] cardPrefabs = Resources.LoadAll<GameObject>(resourcesFolderPath);

        // 2. Loop through each prefab, parse name, create CardData
        foreach (GameObject prefab in cardPrefabs)
        {
            // Create a CardData object from the prefab
            CardData newCardData = CreateCardDataFromPrefab(prefab);
            if (newCardData != null)
            {
                allCards.Add(newCardData);
            }
        }

        // For debugging, print how many cards we loaded
        Debug.Log("Loaded " + allCards.Count + " cards from folder: " + resourcesFolderPath);
        
        deckManager.CreateDeck();
    }

    private CardData CreateCardDataFromPrefab(GameObject prefab)
    {
        // Example prefab name = "Clubs_2" or "Hearts_j"
        string prefabName = prefab.name; 
        string[] parts = prefabName.Split('_');

        // We expect 2 parts: [0] = suit string, [1] = rank string
        if (parts.Length != 2)
        {
            Debug.LogWarning("Prefab name is not in 'Suit_Rank' format: " + prefabName);
            return null;
        }

        string suitString = parts[0];
        string rankString = parts[1];

        // 1) Parse Suit
        Suit suit = ParseSuit(suitString);

        // 2) Parse Rank
        Rank rank = ParseRank(rankString);

        // 3) Get the sprite from the prefab (assuming a SpriteRenderer or Image on it)
        Sprite cardSprite = null;
        
        // If you're using a SpriteRenderer:
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            cardSprite = sr.sprite;
        }
        // If you're using UI Image instead, do:
        // Image img = prefab.GetComponent<Image>();
        // if (img != null) cardSprite = img.sprite;

        // If you still don't find a sprite, log a warning (optional)
        if (cardSprite == null)
        {
            Debug.LogWarning($"No sprite found on prefab {prefabName}");
        }

        // 4) Construct a CardData instance
        //    (We’re just storing it in memory, so a normal 'new' is fine.)
        CardData newCard = new CardData
        {
            suit = suit,
            rank = rank,
            cardSprite = cardSprite
        };

        return newCard;
    }

    private Suit ParseSuit(string suitString)
    {
        // Convert to lowercase for safety
        suitString = suitString.ToLower();

        switch (suitString)
        {
            case "clubs":    return Suit.Clubs;
            case "diamonds": return Suit.Diamonds;
            case "hearts":   return Suit.Hearts;
            case "spades":   return Suit.Spades;
            default:
                Debug.LogWarning("Unknown suit string: " + suitString);
                return Suit.Clubs; // fallback
        }
    }

    private Rank ParseRank(string rankString)
    {
        // Convert to lowercase for safety
        rankString = rankString.ToLower();

        switch (rankString)
        {
            case "2":  return Rank.Two;
            case "3":  return Rank.Three;
            case "4":  return Rank.Four;
            case "5":  return Rank.Five;
            case "6":  return Rank.Six;
            case "7":  return Rank.Seven;
            case "8":  return Rank.Eight;
            case "9":  return Rank.Nine;
            case "10": return Rank.Ten;
            case "j":  return Rank.Jack;
            case "q":  return Rank.Queen;
            case "k":  return Rank.King;
            case "a":  return Rank.Ace;
            default:
                Debug.LogWarning("Unknown rank string: " + rankString);
                return Rank.Two; // fallback
        }
    }
}
