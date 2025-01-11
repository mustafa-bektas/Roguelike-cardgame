using UnityEngine;
using UnityEngine.UI;

public class CardVisualManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject cardDisplayPrefab;
    [SerializeField] private Transform handPanel;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private BattleManager battleManager;

    public CardData DrawAndShowCard()
    {
        CardData drawnCard = deckManager.DrawCard();
        if (drawnCard != null)
        {
            // Instantiate the card in hand
            GameObject newCardObj = Instantiate(cardDisplayPrefab, handPanel);

            CardDisplay display = newCardObj.GetComponent<CardDisplay>();
            display.Initialize(drawnCard, gridManager, true, battleManager); // Mark as drawn this turn

            Button button = newCardObj.GetComponent<Button>();
            button.onClick.AddListener(display.OnCardClick);
        }

        return drawnCard;
    }
}