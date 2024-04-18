using UnityEngine;
using UnityEngine.UI;

public class ScrollingBanner : MonoBehaviour
{
    public float scrollSpeed = 50f;
    private RectTransform _textRectTransform;

    void Start()
    {
        _textRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move the text left
        _textRectTransform.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // If the text is completely scrolled, reset its position
        if (_textRectTransform.anchoredPosition.x <= -_textRectTransform.rect.width)
        {
            _textRectTransform.anchoredPosition = new Vector2(0, _textRectTransform.anchoredPosition.y);
        }
    }
}