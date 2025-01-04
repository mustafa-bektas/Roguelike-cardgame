using UnityEngine;
using UnityEngine.UI;

public class CardVisualManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject cardDisplayPrefab; // Our CardDisplayPrefab
    [SerializeField] private Transform handPanel;          // The parent for spawned cards
    
    [SerializeField] private GridManager gridManager;
    
    // Example method to draw + display one card
    public CardData DrawAndShowCard(int currentTurn)
    {
        CardData drawnCard = deckManager.DrawCard();
        if (drawnCard != null)
        {
            // 1. Instantiate a card display
            GameObject newCardObj = Instantiate(cardDisplayPrefab, handPanel);

            // 2. Get the CardDisplay component
            CardDisplay display = newCardObj.GetComponent<CardDisplay>();
            
            display.Initialize(drawnCard, gridManager, currentTurn); // Pass reference to grid manager
            
            Button button = newCardObj.GetComponent<Button>();
            button.onClick.AddListener(display.OnCardClick);
            
            // group all cards in hand panel by suit then sort by rank
            // get all the cards in the hand panel
            CardDisplay[] cards = handPanel.GetComponentsInChildren<CardDisplay>();
            // sort the cards by suit then by rank
            System.Array.Sort(cards, (a, b) => {
                if (a.GetCardData().suit != b.GetCardData().suit)
                {
                    return a.GetCardData().suit.CompareTo(b.GetCardData().suit);
                }
                return a.GetCardData().rank.CompareTo(b.GetCardData().rank);
            });
            // set the position of the cards
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].transform.SetSiblingIndex(i);
            }
        }

        return drawnCard;
    }
}