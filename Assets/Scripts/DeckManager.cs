using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // We'll reference the CardLoader so we can access 'allCards'
    [SerializeField] private CardLoader cardLoader;

    // This is our working deck (a list of CardData)
    public List<CardData> currentDeck = new List<CardData>();

    private void Start()
    {
        if (cardLoader != null)
        {

        }
        else
        {
            Debug.LogWarning("DeckManager: No CardLoader assigned!");
        }
    }
    
    public void CreateDeck()
    {
        // Clear the current deck
        currentDeck.Clear();
        
        // Copy or clone the allCards list to our currentDeck
        currentDeck.AddRange(cardLoader.allCards);

        // Shuffle the deck so it's in a random order
        ShuffleDeck(currentDeck);
        
        Debug.Log("DeckManager: Created a new deck with " + currentDeck.Count + " cards");
    }

    // Method to shuffle the deck
    private void ShuffleDeck(List<CardData> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            CardData temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    // Draw one card from the top of the deck
    public CardData DrawCard()
    {
        if (currentDeck.Count == 0)
        {
            Debug.LogWarning("DeckManager: Deck is empty!");
            return null;
        }

        CardData topCard = currentDeck[0];
        currentDeck.RemoveAt(0);
        return topCard;
    }

    // (Optional) Method to discard a card or re-insert it at bottom
    public void DiscardCard(CardData card)
    {
        // Or store it in another discard pile if you want
        // For now, let's just drop it from the game
        Debug.Log("Discarding card: " + card.CardName);
    }
}