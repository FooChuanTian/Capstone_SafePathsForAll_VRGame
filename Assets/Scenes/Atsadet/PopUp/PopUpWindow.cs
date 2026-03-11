using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PopUpWindow : MonoBehaviour
{
    public GameObject popUpPanel; // Drag the actual Panel here
    public TextMeshProUGUI whichlanetext;
    
    private Animator animator;
    public bool hasTriggered = false;
    private string currentsceneName;

    void Awake()
    {
        // Get the animator from the PANEL, not this object
        animator = popUpPanel.GetComponent<Animator>();

        // popUpPanel = GameObject.Find("PopUpWindow"); // Ensure this matches the name of your panel in the hierarchy
        
        // Hide the panel, but THIS script stays alive on the parent/Canvas
        popUpPanel.SetActive(false);

        currentsceneName = SceneManager.GetActiveScene().name; // Get the current scene name from the GameNavigationManager
    }

    void Update()
    {   
        Debug.Log("Current lane text: " + whichlanetext.text + hasTriggered); // Debug log to check the current text
        // Now this loop runs every frame because THIS object is active
        if (whichlanetext.text != "Cycling Lane Left" && 
            whichlanetext.text != "Pedestrian Lane Right" && 
            whichlanetext.text != "Pedestrian Lane Left" &&
            currentsceneName != "Cyclist_lesson1")
        {
            if (!hasTriggered) TriggerPopup();
        }
        else if (hasTriggered)
        {
            hasTriggered = false;
            popUpPanel.SetActive(false);
        }
    }

    void TriggerPopup()
    {
        hasTriggered = true;
        popUpPanel.SetActive(true); // Show the panel

        if (animator != null)
        {
            animator.Play("PopUpFade"); 
        }
    }

}