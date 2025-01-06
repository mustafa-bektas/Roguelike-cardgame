using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("Card Display Settings")]
    [SerializeField] private Image cardImage;              // Display card image
    [SerializeField] private Outline selectionOutline;     // Outline effect for selection
    
    [Header("Card Properties")]
    public CardData cardData;                              // Data for the card
    public bool drawnOnTurn = false;                       // Track if drawn this turn
    public int row = -1;                                   // Grid row position (-1 means not placed)
    public int col = -1;                                   // Grid column position (-1 means not placed)

    private bool isSelected = false;                       // Track if card is selected
    private GridManager gridManager;                       // Reference to the GridManager

    // Hover/Select Scaling Variables
    private Vector3 originalScale;                         // Original scale
    private Vector3 hoverScale;                            // Hover effect scale
    private Vector3 selectScale;                           // Selected effect scale

    // **Initialization**
    public void Initialize(CardData data, GridManager gridManagerRef, bool drawnThisTurn)
    {
        cardData = data;
        gridManager = gridManagerRef;
        drawnOnTurn = drawnThisTurn;
        UpdateVisuals();

        // Cache original scale and calculate hover effects
        originalScale = transform.localScale;
        hoverScale = originalScale * 1.1f;  // Slightly larger hover
        selectScale = originalScale * 1.2f; // Larger for selection
    }

    // **Update Visuals**
    private void UpdateVisuals()
    {
        if (cardData != null && cardData.cardSprite != null)
        {
            cardImage.sprite = cardData.cardSprite;
        }
    }

    // **Get Card Data**
    public CardData GetCardData()
    {
        return cardData;
    }

    // **Left-Click: Select the Card**
    public void OnCardClick()
    {
        if (isSelected)
        {
            DeselectCard(); // Unselect if already selected
        }
        else
        {
            SelectCard(); // Select the card
        }
    }

    // **Selection**
    public void SelectCard()
    {
        // Clear any previous selection in the grid manager
        if (gridManager.HasSelectedCard())
        {
            gridManager.ClearSelectedCard();
        }

        isSelected = true;
        gridManager.SelectCard(this);

        // Apply visual effects
        if (selectionOutline != null) selectionOutline.enabled = true;
        transform.localScale = selectScale; // Enlarge when selected
    }

    public void DeselectCard()
    {
        isSelected = false;

        // Remove visual effects
        if (selectionOutline != null) selectionOutline.enabled = false;
        transform.localScale = originalScale; // Reset size
    }

    // **Hover Effects**
    public void OnMouseEnter()
    {
        if (!isSelected)
        {
            transform.localScale = hoverScale; // Slightly enlarge on hover
        }
    }

    public void OnMouseExit()
    {
        if (!isSelected)
        {
            transform.localScale = originalScale; // Reset size on hover exit
        }
    }

    // **RIGHT-CLICK: Remove or Return Card**
    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click detected
        {
            // Check if this card is under the mouse
            Vector3 mousePos = Input.mousePosition;
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos, Camera.main))
            {
                HandleCardRemovalFromGrid(); // Process removal
            }
        }
    }

    // **Handle Card Removal or Return**
    private void HandleCardRemovalFromGrid()
    {
        if (row == -1 || col == -1)
        {
            Debug.LogWarning("Card is not in the grid.");
            return;
        }

        if (drawnOnTurn)
        {
            // Return the card to the hand if drawn this turn
            gridManager.ReturnCardToHand(this);
        }
        else
        {
            // Destroy the card if it was already in the grid
            gridManager.RemoveCardFromGrid(this);
        }
    }

    // **Update Card Position in Grid**
    public void UpdateGridPosition(int newRow, int newCol)
    {
        row = newRow;
        col = newCol;
    }
}
