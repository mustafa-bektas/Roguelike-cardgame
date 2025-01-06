using UnityEngine;
using UnityEngine.UI;

public class GridSlot : MonoBehaviour
{
    public int row; // Row position in the grid
    public int col; // Column position in the grid
    private GridManager gridManager;

    public void Initialize(int row, int col, GridManager manager)
    {
        this.row = row;
        this.col = col;
        gridManager = manager;

        // Attach click listener
        transform.GetComponent<Button>().onClick.AddListener(OnSlotClick);
    }

    private void OnSlotClick()
    {
        // Attempt to place a card in this slot
        if (gridManager.HasSelectedCard())
        {
            bool success = gridManager.PlaceCardInSlot(row, col);
            if (success)
            {
                Debug.Log($"Card placed at ({row}, {col}).");
            }
        }
        else
        {
            Debug.Log("No card selected for placement.");
        }
    }
}