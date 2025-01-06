using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LogsPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform logsPanel;   // Logs Panel RectTransform
    [SerializeField] private TextMeshProUGUI logsText;  // Text area to display logs
    [SerializeField] private ScrollRect scrollRect;     // For scrolling logs automatically

    [Header("Animation Settings")]
    [SerializeField] private float slideSpeed = 500f;   // Speed of sliding animation
    private bool isVisible = false;                     // Tracks visibility state
    private Vector2 hiddenPosition;                     // Position when hidden
    private Vector2 visiblePosition;                    // Position when visible

    private void Awake()
    {
        // Set the panel's initial positions
        visiblePosition = logsPanel.anchoredPosition + new Vector2(logsPanel.rect.width, 0);
        hiddenPosition = visiblePosition - new Vector2(logsPanel.rect.width, 0);
        logsPanel.anchoredPosition = hiddenPosition;

        // Listen for log messages
        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        Application.logMessageReceived -= HandleLog;
    }

    // Logs incoming messages into the text area
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
                logsText.text += $"<color=#FF4C4C>{logString}</color>\n"; // Red for errors
                break;
            case LogType.Warning:
                logsText.text += $"<color=#FFD700>{logString}</color>\n"; // Yellow for warnings
                break;
            default:
                logsText.text += $"{logString}\n"; // Default white for normal logs
                break;
        }

        // Auto-scroll to the bottom
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    // Toggles panel visibility
    public void ToggleLogsPanel()
    {
        if (isVisible)
        {
            // Slide out to hide
            StartCoroutine(SlidePanel(hiddenPosition));
        }
        else
        {
            // Slide in to show
            StartCoroutine(SlidePanel(visiblePosition));
        }
        isVisible = !isVisible;
    }

    // Handles the sliding animation
    private System.Collections.IEnumerator SlidePanel(Vector2 targetPosition)
    {
        while (Vector2.Distance(logsPanel.anchoredPosition, targetPosition) > 0.1f)
        {
            logsPanel.anchoredPosition = Vector2.MoveTowards(
                logsPanel.anchoredPosition,
                targetPosition,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Ensure it snaps exactly to the target position
        logsPanel.anchoredPosition = targetPosition;
    }
}
