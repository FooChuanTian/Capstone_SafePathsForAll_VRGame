using TMPro;
using UnityEngine;

public class TutorialPanelSimulation : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI tutorialText;
    public GameObject TutorialPanel;
    private string textToShow;
    void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        string playerType = player.GetComponent<ScoreManager>().PlayerType;
        if (playerType == "pedestrian")
        {
            if (PedestrianGameNavigationManager.Instance != null)
            {
                textToShow = "Now that you have learnt all about path safety, let's test your skills! Get to the other side of the path safely! Remember to keep yourself and others safe while riding!\n\nPress X to continue";
            }
            else
            {
                textToShow = "Welcome! Your task is to get to the other end of this shared path safely! But be careful, don't bump into the other otters!\n\nPress X to continue";
            }
        }
        else if (playerType == "cyclist")
        {
            if (GameNavigationManager.Instance != null)
            {
                textToShow = "Now that you have learnt all about path safety, let's test your skills! Get to the other side of the path safely! Stay safe!\n\nPress X to continue";
            }
            else
            {
                textToShow = "Welcome! Your task is to get to the other end of this shared path safely! But be careful, don't knock into the other otters!\n\nPress X to continue";
            }
        }
        tutorialText.text = textToShow;

    }

    public void CloseTutorialPanel()
    {
        TutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            CloseTutorialPanel();
        }
    }

}
