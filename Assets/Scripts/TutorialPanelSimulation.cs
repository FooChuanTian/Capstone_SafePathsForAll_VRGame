using TMPro;
using UnityEngine;

public class TutorialPanelSimulation : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI tutorialText;
    public GameObject TutorialPanel;
    public GameObject camera;
    private string textToShow;
    public bool isVR = false;
    void Start()
    {
        Time.timeScale = 0f;
        if (isVR && camera!=null) camera.GetComponent<CustomCameraRig>().PublicUpdateAnchors(true, true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        string playerType = player.GetComponent<ScoreManager>().PlayerType;
        if (playerType == "pedestrian")
        {
            if (PedestrianGameNavigationManager.Instance != null)
            {
                textToShow = "This is the tutorial for last pedestrian simulation";
            }
            else
            {
                textToShow = "This is the tutorial for first pedestrian simulation";
            }
        }
        else if (playerType == "cyclist")
        {
            if (GameNavigationManager.Instance != null)
            {
                textToShow = "This is the tutorial for last AMD simulation";
            }
            else
            {
                textToShow = "This is the tutorial for first AMD simulation";
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

}
