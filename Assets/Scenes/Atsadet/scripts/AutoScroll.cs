using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;
    private RectTransform rectTransform;
    private bool isStopped = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Move the text up over time
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        
        // Optional: Loop back to bottom if it goes too high
        // if (rectTransform.anchoredPosition.y > -1700f) 
        // {
        //     rectTransform.anchoredPosition = new Vector2(0, -3563f);
        // }
        if (rectTransform.anchoredPosition.y > -2000f) 
        {
            scrollSpeed = 0;
            isStopped = true;
        }

        if (isStopped && OVRInput.GetDown(OVRInput.Button.Three))
        {   
            GameObject[] persistentObjects = GameObject.FindGameObjectsWithTag("PersistentObj");
            foreach (GameObject obj in persistentObjects)
            {
                Destroy(obj);
                Debug.Log("Destroyed " + obj.name);
            }
            SceneManager.LoadScene("PlayerSelectionScene");
        }
    }
}