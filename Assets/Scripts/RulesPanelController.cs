using UnityEngine;
using UnityEngine.UI;

public class RulesPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform rulesPanel; // Reference to the panel's RectTransform
    [SerializeField] private float slideDuration = 0.3f; // Sliding animation duration

    private bool isPanelOpen = false; // Track whether the panel is open
    private Vector2 closedPosition;  // Position when hidden
    private Vector2 openPosition;    // Position when visible

    private void Start()
    {
        // Calculate the panel positions
        closedPosition = new Vector2(Screen.width, 0); // Off-screen to the right
        openPosition = new Vector2((Screen.width - rulesPanel.rect.width)/2, 0); // Visible position

        // Start with the panel hidden
        rulesPanel.anchoredPosition = closedPosition;
    }

    // Toggle panel visibility
    public void ToggleRulesPanel()
    {
        if (isPanelOpen)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }

    public void OpenPanel()
    {
        isPanelOpen = true;
        StopAllCoroutines(); // Ensure no conflicting animations
        StartCoroutine(SlidePanel(openPosition));
    }

    public void ClosePanel()
    {
        isPanelOpen = false;
        StopAllCoroutines(); // Ensure no conflicting animations
        StartCoroutine(SlidePanel(closedPosition));
    }

    private System.Collections.IEnumerator SlidePanel(Vector2 targetPosition)
    {
        Vector2 startPosition = rulesPanel.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / slideDuration);
            rulesPanel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        rulesPanel.anchoredPosition = targetPosition; // Snap to final position
    }
}