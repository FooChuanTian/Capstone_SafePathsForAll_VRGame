using UnityEngine;

public class PopUpWindow_Pedestrian : MonoBehaviour
{
    public GameObject popUpPanel;

    private Animator animator;
    private bool isShowing = false;

    void Awake()
    {
        animator = popUpPanel.GetComponent<Animator>();
        popUpPanel.SetActive(false);
    }

    public void ShowWarning()
    {
        if (isShowing) return;

        isShowing = true;
        popUpPanel.SetActive(true);

        if (animator != null)
            animator.Play("PopUpFade");
    }

    public void HideWarning()
    {
        if (!isShowing) return;

        isShowing = false;
        popUpPanel.SetActive(false);
    }
}