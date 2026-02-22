using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PopUpWindow : MonoBehaviour
{
    public TextMeshProUGUI popUpText;
    private GameObject popUpWindow;
    private Animator popUpAnimator;

    private Queue<string> popUpQueue;
    private bool isActive;
    private Coroutine queueChecker;
    public TextMeshProUGUI whichlanetext;
    private float nextAllowedPopupTime = 0f; // Stores when we can next show a message
    public float cooldownDuration = 5.0f;    // The 5-second delay

    private void Start()
    {
        popUpWindow = transform.GetChild(0).gameObject;
        popUpAnimator = popUpWindow.GetComponent<Animator>();
        popUpQueue = new Queue<string>();
        popUpWindow.SetActive(false);
    }
    public void AddToQueue(string text){
        popUpQueue.Enqueue(text);
        if (queueChecker == null) {
            queueChecker = StartCoroutine(CheckQueue());
        }
    }
    private void ShowPopup(string text)
    {
        isActive = true;
        popUpWindow.SetActive(true);
        popUpText.text = text;
        popUpAnimator.Play("PopUpAnimation");
    }

    private IEnumerator CheckQueue() {
        do {
            ShowPopup(popUpQueue.Dequeue());
            do {
               yield return null; 
            } while (!popUpAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));

        } while (popUpQueue.Count > 0);
        popUpWindow.SetActive(false);
        queueChecker = null;
    }

    void Update()
    {
        // 1. Check if the lane is wrong
        if (whichlanetext.text != "Cycling Lane Left")
        {
            // 2. Check if the current time has passed our "Next Allowed" threshold
            if (Time.time >= nextAllowedPopupTime)
            {
                AddToQueue("Wrong lane!");

                // 3. Set the new threshold to (Now + 5 seconds)
                nextAllowedPopupTime = Time.time + cooldownDuration;
            }
        }
    }
}
