using System;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    // The Image component that shows the card sprite
    [SerializeField] private Image cardImage;

    public CardData cardData;
    private GridManager gridManager; // Reference to GridManager
    private bool isInGrid = false; // Track if placed in the grid
    private int currentRow, currentCol; // Track grid position
    private int drawnOnTurn = -1; // Track when card was drawn

    public void Initialize(CardData data, GridManager gridManagerRef, int turn)
    {
        cardData = data;
        gridManager = gridManagerRef;
        UpdateVisuals();
        drawnOnTurn = turn;
    }

    private void UpdateVisuals()
    {
        if (cardData != null && cardData.cardSprite != null)
        {
            cardImage.sprite = cardData.cardSprite;
        }
    }

    // (Optional) Provide a way to get the CardData if needed
    public CardData GetCardData()
    {
        return cardData;
    }

    public void OnCardClick()
    {
        if (isInGrid)
        {
            if (drawnOnTurn == BattleManager.currentTurn)
            {
                gridManager.PutCardBackInHand(this);
            }
            else
            {
                // Remove from grid
                gridManager.RemoveCardFromSlot(currentRow, currentCol);
            }

            isInGrid = false;
        }
        else
        {
            // Try to place into grid
            for (int row = 0; row < gridManager.numRows; row++)
            {
                for (int col = 0; col < gridManager.numColumns; col++)
                {
                    if (gridManager.IsSlotEmpty(row, col))
                    {
                        bool placed = gridManager.PlaceCardInSlot(this, row, col);
                        if (placed)
                        {
                            isInGrid = true;
                            currentRow = row;
                            currentCol = col;
                        }
                        return;
                    }
                }
            }

            Debug.Log("No empty slot available!");
        }
    }
    
}