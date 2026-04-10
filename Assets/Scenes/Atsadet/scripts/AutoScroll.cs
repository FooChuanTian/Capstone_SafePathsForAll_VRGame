using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move the text up over time
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        
        // Optional: Loop back to bottom if it goes too high
        if (rectTransform.anchoredPosition.y > 3200f) 
        {
            rectTransform.anchoredPosition = new Vector2(0, -3593f);
        }
    }
}