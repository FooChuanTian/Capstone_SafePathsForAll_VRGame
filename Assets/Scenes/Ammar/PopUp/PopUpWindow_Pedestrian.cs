using System.Collections;
using UnityEngine;

public class PopUpWindow_Pedestrian : MonoBehaviour
{
    public GameObject popUpPanel;

    private CanvasGroup canvasGroup;
    private bool isShowing = false;
    private Coroutine flashCoroutine;

    public float flashSpeed = 3f; // Higher = faster flashing

    void Awake()
    {
        canvasGroup = popUpPanel.GetComponent<CanvasGroup>();
        popUpPanel.SetActive(false);
    }

    public void ShowWarning()
    {
        if (isShowing) return;

        isShowing = true;
        popUpPanel.SetActive(true);

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashWarning());
    }

    public void HideWarning()
    {
        if (!isShowing) return;

        isShowing = false;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        popUpPanel.SetActive(false);

        // Reset alpha for next time
        if (canvasGroup != null)
            canvasGroup.alpha = 0.7f;
    }

    IEnumerator FlashWarning()
    {
        while (isShowing)
        {
            // Fade in
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * flashSpeed;
                if (canvasGroup != null)
                    canvasGroup.alpha = Mathf.Lerp(0.2f, 1f, t);
                yield return null;
            }

            // Fade out
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * flashSpeed;
                if (canvasGroup != null)
                    canvasGroup.alpha = Mathf.Lerp(1f, 0.2f, t);
                yield return null;
            }
        }
    }
}