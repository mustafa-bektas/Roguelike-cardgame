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
        // Initialize synergy totals
        int totalDamage = 0;
        int totalHealing = 0;
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

                // Check neighbors
                foreach (var offset in neighborOffsets)
                {
                    int neighborRow = row + offset[0];
                    int neighborCol = col + offset[1];

                    // Ensure neighbor is within bounds
                    if (neighborRow >= 0 && neighborRow < numRows && neighborCol >= 0 && neighborCol < numColumns)
                    {
                        CardDisplay neighborCard = gridManager.GetCardAt(neighborRow, neighborCol);
                        if (neighborCard == null) continue; // Skip empty neighbors

                        CardData neighborData = neighborCard.GetCardData();
                        SynergyResult result = ComputeCardSynergy(currentData, neighborData);

                        // Accumulate results
                        totalDamage += result.damage;
                        totalHealing += result.healing;
                        totalShield += result.shield;
                    }
                }
            }
        }

        // Log final results
        Debug.Log($"Total Damage: {totalDamage}, Healing: {totalHealing}, Shield: {totalShield}");

        // Return the calculated synergy
        return new SynergyResult
        {
            damage = totalDamage,
            healing = totalHealing,
            shield = totalShield
        };
    }

    // Define neighbor offsets for adjacency (orthogonal and diagonal)
    private int[][] neighborOffsets = new int[][]
    {
        new int[] {0, 1},  // Right
        new int[] {1, 0},  // Down
        new int[] {1, 1},  // Down-right
        new int[] {1, -1}, // Down-left
    };

    // Computes synergy rules between two cards
    private SynergyResult ComputeCardSynergy(CardData a, CardData b)
    {
        var damage = 0;
        var healing = 0;
        var shield = 0;

        int rankA = (int)a.rank; // Use rank values directly
        int rankB = (int)b.rank;

        // Rule 1: Same Suit → Damage
        if (a.suit == b.suit)
        {
            damage += Mathf.Max(rankA, rankB); // Add the highest rank as damage
            Debug.Log($"Same suit! Damage +{Mathf.Max(rankA, rankB)}");

            // Rule 2: Consecutive Ranks AND Same Suit → Bonus Damage
            if (Mathf.Abs(rankA - rankB) == 1)
            {
                damage *= 2; // Double the damage as bonus
                Debug.Log("Consecutive ranks! Bonus Damage x2");
            }
        }

        // Rule 3: Same Rank → Shield
        if (rankA == rankB)
        {
            shield += rankA; // Rank value as shield
            Debug.Log($"Same rank! Shield +{rankA}");
        }

        // Rule 4: Face Cards (J, Q, K) → if same rank and red -> heal, else damage
        if (rankA >= (int)Rank.Jack || rankB >= (int)Rank.Jack)
        {
            if (a.suit == Suit.Diamonds || a.suit == Suit.Hearts ||
                b.suit == Suit.Diamonds || b.suit == Suit.Hearts)
            {
                if (a.rank == b.rank)
                {
                    shield *= 2; // Double the healing
                    Debug.Log("Face cards same rank and red! Shield x2");
                }
            }
            else
            {
                if (a.rank == b.rank)
                {
                    damage *= 2; // Double the damage
                    Debug.Log("Face cards same rank and black! Damage x2");
                }
            }
        }

        return new SynergyResult
        {
            damage = damage,
            healing = healing,
            shield = shield
        };
    }

    // A small struct to store synergy results
    public struct SynergyResult
    {
        public int damage;
        public int healing;
        public int shield;
    }
}
