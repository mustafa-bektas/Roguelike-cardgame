using System;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] public int numRows = 2;
    [SerializeField] public int numColumns = 3;
    [SerializeField] private Vector2 cellSize = new Vector2(218, 264); // Cell size
    [SerializeField] private Vector2 gridOrigin = new Vector2(-250, 100); // Position in UI
    [SerializeField] private Transform handPanel;          // The parent for spawned cards

    private CardDisplay[,] gridSlots; // Tracks cards in grid
    private Vector2[,] slotPositions; // Tracks positions
    
    private int lastHighlightedX = 0;
    private int lastHighlightedY = 0;

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        // Initialize arrays
        gridSlots = new CardDisplay[numRows, numColumns];
        slotPositions = new Vector2[numRows, numColumns];

        // Calculate slot positions based on grid
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                slotPositions[row, col] = gridOrigin + new Vector2(col * cellSize.x, -row * cellSize.y);
            }
        }
        
        HighlightEmptySlot();
    }

    // Adds a card to a specific slot
    public bool PlaceCardInSlot(CardDisplay card, int row, int col)
    {
        if (gridSlots[row, col] != null) // Slot is occupied
        {
            Debug.Log($"Slot ({row}, {col}) is already occupied!");
            return false;
        }

        // Place card
        gridSlots[row, col] = card;
        card.transform.SetParent(transform, false); // Parent to grid
        card.transform.localPosition = slotPositions[row, col]; // Move to slot position
        Debug.Log($"Placed {card.cardData.CardName} at ({row}, {col})");
        
        HighlightEmptySlot();
        return true;
    }

    // Removes a card from the grid
    public bool RemoveCardFromSlot(int row, int col)
    {
        if (gridSlots[row, col] == null) // No card here
        {
            Debug.Log($"No card to remove at ({row}, {col})!");
            return false;
        }

        // Remove card
        Destroy(gridSlots[row, col].gameObject); // Destroy the GameObject
        gridSlots[row, col] = null; // Clear the slot
        Debug.Log($"Removed card from ({row}, {col})");
        
        HighlightEmptySlot();
        return true;
    }
    
    public bool PutCardBackInHand(CardDisplay card)
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (gridSlots[row, col] == card)
                {
                    gridSlots[row, col] = null;
                    card.transform.SetParent(handPanel.transform, false);
                    HighlightEmptySlot();
                    return true;
                }
            }
        }
        return false;
    }

    // Check if a slot is empty
    public bool IsSlotEmpty(int row, int col)
    {
        return gridSlots[row, col] == null;
    }

    // Get the card at a specific slot
    public CardDisplay GetCardAt(int row, int col)
    {
        return gridSlots[row, col];
    }

    // Clear all cards from the grid
    public void ClearGrid()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (gridSlots[row, col] != null)
                {
                    Destroy(gridSlots[row, col].gameObject);
                    gridSlots[row, col] = null;
                }
            }
        }
    }
    
    public void RemoveThreeRandomCards()
    {
        if (GetNumberOfCardsInGrid() > 3)
        {
            int removed = 0;
        
            while (removed < 3)
            {
                int row = UnityEngine.Random.Range(0, numRows);
                int col = UnityEngine.Random.Range(0, numColumns);
                if (gridSlots[row, col] != null)
                {
                    Destroy(gridSlots[row, col].gameObject);
                    gridSlots[row, col] = null;
                    removed++;
                }
            }
            HighlightEmptySlot();
        }
    }
    
    private int GetNumberOfCardsInGrid()
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

    private void HighlightEmptySlot()
    {
        DehighlightLastHighlightedSlot();
        // find the first empty slot
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (gridSlots[row, col] == null)
                {
                    GameObject slot = GameObject.Find($"{row}{col}");
                    if (slot != null)
                    {
                        slot.GetComponent<Image>().color = new Color32(255, 251, 134, 255);
                        lastHighlightedX = row;
                        lastHighlightedY = col;
                        return;
                    }
                }
            }
        }
    }

    private void DehighlightLastHighlightedSlot()
    {
        GameObject slot = GameObject.Find($"{lastHighlightedX}{lastHighlightedY}");
        if (slot != null)
        {
            slot.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
    
}
