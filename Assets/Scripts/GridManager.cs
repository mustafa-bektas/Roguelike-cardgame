using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int numRows = 2;
    public int numColumns = 3;

    private CardDisplay[,] gridSlots; // 2D array for cards in grid
    private CardDisplay selectedCard; // The card currently selected for placement
    [SerializeField] private Transform handPanel;

    private void Awake()
    {
        gridSlots = new CardDisplay[numRows, numColumns];
    }
    
    private void Start()
    {
        // Automatically initialize grid slots
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                // Assume each slot is a child of the GridManager in the Unity hierarchy
                Transform slotTransform = transform.GetChild(row * numColumns + col);
                GridSlot slot = slotTransform.gameObject.AddComponent<GridSlot>();
                slot.Initialize(row, col, this);
            }
        }
    }

    // Select a card from hand
    public void SelectCard(CardDisplay card)
    {
        selectedCard = card;
    }

    // Clear selected card
    public void ClearSelectedCard()
    {
        if (selectedCard != null)
        {
            selectedCard.DeselectCard();
            selectedCard = null;
        }
    }

    public bool HasSelectedCard()
    {
        return selectedCard != null;
    }

    // Place card in the grid
    public bool PlaceCardInSlot(int row, int col)
    {
        Debug.Log($"Placing card at row {row}, col {col}");

        // Error handling: Make sure a card is selected
        if (selectedCard == null)
        {
            Debug.LogWarning("No card selected!");
            return false;
        }

        // Error handling: Check if the slot is already occupied
        if (gridSlots[row, col] != null)
        {
            Debug.LogWarning("Slot already occupied! Select an empty slot.");
            return false;
        }
        
        // Prevent moving cards already placed on the grid in previous turns
        if (!selectedCard.drawnOnTurn)
        {
            Debug.LogWarning("Cannot move cards already placed in previous turns!");
            return false;
        }

        // Clear the old slot if the card was already in the grid
        if (selectedCard.row != -1 && selectedCard.col != -1)
        {
            // Mark the old slot as empty
            gridSlots[selectedCard.row, selectedCard.col] = null;
        }

        // Place the selected card in the new slot
        gridSlots[row, col] = selectedCard;

        // Update the card's position in the grid
        selectedCard.UpdateGridPosition(row, col);
        selectedCard.transform.SetParent(transform, false); // Move card to grid parent
        selectedCard.transform.localPosition = GetSlotPosition(row, col);

        // Deselect the card after placement
        ClearSelectedCard();
        return true;
    }

    public bool IsSlotEmpty(int row, int col)
    {
        return gridSlots[row, col] == null;
    }

    public CardDisplay GetCardAt(int row, int col)
    {
        return gridSlots[row, col];
    }

    private Vector3 GetSlotPosition(int row, int col)
    {
        float xOffset = col * 218; // Adjust grid size based on card width
        float yOffset = -(row * 264);

        if (row == 0)
        {
            yOffset += 36;
        }
        else
        {
            yOffset -= 36;
        }
        
        return new Vector3(xOffset, yOffset, 0);
    }
    
    public void RemoveTwoRandomCards()
    {
        // Collect valid occupied slots
        List<(int, int)> occupiedSlots = new List<(int, int)>();
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (gridSlots[row, col] != null)
                {
                    occupiedSlots.Add((row, col));
                }
            }
        }

        // Ensure we have enough cards to remove
        if (occupiedSlots.Count > 3)
        {
            for (int i = 0; i < 2; i++)
            {
                int index = Random.Range(0, occupiedSlots.Count);
                (int row, int col) = occupiedSlots[index];
                occupiedSlots.RemoveAt(index);

                CardDisplay cardToRemove = gridSlots[row, col];
                Destroy(cardToRemove.gameObject);
                gridSlots[row, col] = null;
            }
        }
    }

    private int GetNumberOfCardsOnTheGrid()
    {
        int count = 0;
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (gridSlots[row, col] != null)
                {
                    count++;
                }
            }
        }
        return count;
    }
    
    // **Return a card to the hand**
    public void ReturnCardToHand(CardDisplay card)
    {
        // Clear grid slot reference
        gridSlots[card.row, card.col] = null;

        // Move the card back to the hand
        card.transform.SetParent(handPanel, worldPositionStays: false);
        card.DeselectCard(); // Reset its visuals
        card.row = -1;
        card.col = -1; // Clear position

        Debug.Log($"Card {card.cardData.CardName} returned to hand.");
    }

    // **Remove a card from the grid**
    public void RemoveCardFromGrid(CardDisplay card)
    {
        // Clear grid slot reference
        gridSlots[card.row, card.col] = null;

        Destroy(card.gameObject);
        Debug.Log($"Card {card.cardData.CardName} removed from grid.");
    }
    
    public void MarkAllCardsOnTheGridAsNotDrawnOnTurn()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (gridSlots[row, col] != null)
                {
                    gridSlots[row, col].drawnOnTurn = false;
                }
            }
        }
    }
}
