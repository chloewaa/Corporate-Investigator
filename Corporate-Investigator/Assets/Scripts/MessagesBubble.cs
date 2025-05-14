using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class MessagesBubble : MonoBehaviour {
    [Header("Text Reference")]
    public TextMeshProUGUI messageText;

    [Header("Sizing Settings")]
    [Tooltip("Maximum width of the bubble in pixels before wrapping")]
    public float maxWidth = 400f;
    [Tooltip("Total horizontal padding (left + right) in pixels")]
    public float paddingX = 20f;
    [Tooltip("Total vertical padding (top + bottom) in pixels")]
    public float paddingY = 10f;

    private RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start() {
        ResizeBubble();
    }

    public void ResizeBubble() {
        if (messageText == null) {
            Debug.LogWarning("MessageBubble: messageText is not assigned.", this);
            return;
        }

        //Ensure text wraps at word boundaries
        messageText.enableWordWrapping = true;  
        messageText.ForceMeshUpdate();

        //Calculate preferred values with a width cap
        Vector2 textSize = messageText.GetPreferredValues(
            messageText.text,
            maxWidth,
            Mathf.Infinity
        );

        //Determine final dimensions
        float finalWidth  = Mathf.Min(textSize.x, maxWidth) + paddingX;
        float finalHeight = textSize.y + paddingY;

        //Apply to the bubble's RectTransform
        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            finalWidth
        );
        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            finalHeight
        );
    }
}
