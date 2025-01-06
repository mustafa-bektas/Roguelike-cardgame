using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Managers & References")]
    [SerializeField] private CardVisualManager cardVisualManager;
    [SerializeField] private SynergyCalculator synergyCalculator;
    [SerializeField] private GridManager gridManager;
    
    [Header("UI References")]
    [SerializeField] private TMPro.TextMeshProUGUI playerHpText;
    [SerializeField] private TMPro.TextMeshProUGUI enemyHpText;

    [Header("Game Variables")]
    public int playerHP = 30;
    public int playerShield = 0;
    public int enemyHP = 40;
    public int enemyFlatDamage = 15;

    // Maybe you want to draw 3-5 cards at the start of each turn
    [SerializeField] private int cardsDrawnPerTurn = 3;
    
    public static int currentTurn = 0;

    private void Start()
    {
        // Initialize HP UI
        UpdateHPUI();
        // Possibly do an initial "Start Turn" automatically
        StartTurn();
    }

    public void StartTurn()
    {
        gridManager.RemoveTwoRandomCards();
        currentTurn++;
        // 1. Draw some cards
        for (int i = 0; i < cardsDrawnPerTurn; i++)
        {
            CardData drawnCard = cardVisualManager.DrawAndShowCard();
        }
        // Player places cards, etc...
        // This continues until player hits "End Turn"
    }

    // This is called by an "End Turn" button
    public void EndTurn()
    {
        // 1. Calculate synergy from the synergy grid
        SynergyCalculator.SynergyResult synergy = synergyCalculator.CalculateSynergy();

        // For demonstration, let's break synergy into simpler categories:
        // (You can expand synergyCalculator to return separate values for damage, healing, etc.)
        // But let's assume for now "totalSynergy" = damage to enemy, 
        // or you might have synergyCalculator produce a SynergyResult object with multiple fields.

        // 2. Apply player's synergy effects
        // Deal damage to enemy
        enemyHP -= synergy.damage;
        if (enemyHP < 0) enemyHP = 0;
        
        // 3. Check win/loss conditions
        if (enemyHP <= 0)
        {
            UpdateHPUI();
            Debug.Log("You won! Enemy HP is 0.");
            // Possibly load next floor or show "Victory" screen
        }
        
        playerShield += synergy.shield;

        // 4. Enemy deals flat damage
        int damageToPlayer = enemyFlatDamage;
        Debug.LogWarning("Enemy deals " + damageToPlayer + " damage to player");
        if (playerShield >= damageToPlayer)
        {
            // Shield absorbs it fully
            playerShield -= damageToPlayer;
            damageToPlayer = 0;
        }
        else
        {
            // Some leftover hits player's HP
            damageToPlayer -= playerShield;
            playerShield = 0;
            playerHP -= damageToPlayer;
        }
        

        // 4. Check win/loss conditions
        if (enemyHP <= 0)
        {
            UpdateHPUI();
            Debug.Log("You won! Enemy HP is 0.");
            // Possibly load next floor or show "Victory" screen
        }
        else if (playerHP <= 0)
        {
            UpdateHPUI();
            Debug.Log("You lost! Player HP is 0.");
            // Show "Game Over" screen or something
        }
        else
        {
            // Continue to next turn
            // Possibly discard or keep the synergy grid as is, 
            // or clear the grid if that's your design
            
            //ClearSynergyGrid(); // if you want to remove cards from the grid
            
            // Then start the next turn
            UpdateHPUI();
            gridManager.MarkAllCardsOnTheGridAsNotDrawnOnTurn();
            StartTurn();
        }
    }

    private void UpdateHPUI()
    {
        playerHpText.text = "Player HP: " + playerHP + "\nShield: " + playerShield;
        enemyHpText.text = "Enemy HP: " + enemyHP;
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
