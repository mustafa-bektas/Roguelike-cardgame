using UnityEngine;

public class SynergyCalculator : MonoBehaviour
{
    [Header("Grid Reference")]
    [SerializeField] private GridManager gridManager;

    [Header("Grid Dimensions")]
    [SerializeField] public int numRows = 2;
    [SerializeField] public int numColumns = 3;

    public SynergyResult CalculateSynergy()
    {
        // Initialize totals
        int totalDamage = 0;
        int totalShield = 0;

        // Iterate through all grid slots
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                // Get the current card
                CardDisplay currentCard = gridManager.GetCardAt(row, col);
                if (currentCard == null) continue; // Skip empty slots

                CardData currentData = currentCard.GetCardData();
                int rank = (int)currentData.rank; // Get the card rank

                // Determine card type
                bool isAttackCard = (currentData.suit == Suit.Hearts || currentData.suit == Suit.Diamonds);
                bool isShieldCard = (currentData.suit == Suit.Spades || currentData.suit == Suit.Clubs);

                Debug.LogWarning($"Evaluating {currentData.CardName} at Row {row}, Col {col}");

                // Evaluate based on row and card type
                if (row == 0) // Front row (Attack row)
                {
                    if (isAttackCard)
                    {
                        totalDamage += rank; // Full damage for attack cards
                        Debug.Log($"{currentData.CardName} deals {rank} damage!");
                    }
                    else if (isShieldCard)
                    {
                        totalShield += rank / 2; // Half shield for shield cards
                        Debug.Log($"{currentData.CardName} applies {rank / 2} shield!");
                    }
                }
                else if (row == 1) // Back row (Defense row)
                {
                    if (isShieldCard)
                    {
                        totalShield += rank; // Full shield for shield cards
                        Debug.Log($"{currentData.CardName} applies {rank} shield!");
                    }
                    else if (isAttackCard)
                    {
                        totalDamage += rank / 2; // Half damage for attack cards
                        Debug.Log($"{currentData.CardName} deals {rank / 2} damage!");
                    }
                }
            }
        }

        // Log final results
        Debug.LogWarning($"Total Damage: {totalDamage}, Total Shield: {totalShield}");

        // Return the calculated synergy
        return new SynergyResult
        {
            damage = totalDamage,
            shield = totalShield
        };
    }

    // A small struct to store synergy results
    public struct SynergyResult
    {
        public int damage;
        public int shield;
    }
}
