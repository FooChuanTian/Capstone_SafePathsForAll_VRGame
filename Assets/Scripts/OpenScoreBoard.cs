using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;

public class OpenScoreBoard : MonoBehaviour
{
    public Transform ScoreBoardObject;
    public Transform ScoreBoardVR;
    public bool isPedestrianSimulation = true;
    private bool hasOpened = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        if (hasOpened) return;
        hasOpened = true;

        bool isFinalSim = isPedestrianSimulation
            ? PedestrianGameNavigationManager.Instance != null
            : GameNavigationManager.Instance != null;

        Time.timeScale = 0;
        //ScoreBoardObject.gameObject.SetActive(true);
        if (XRSettings.enabled)
        {
            ScoreBoardVR.gameObject.SetActive(true);
        }

        // Show or hide scoreboard list based on whether it's the final sim
        //Transform scoreboardList = ScoreBoardObject.Find("ScoreboardList");
        //if (scoreboardList != null)
            //scoreboardList.gameObject.SetActive(isFinalSim);

        // Refresh scoreboard entries now that it's visible
        ScoreBoardManager manager = ScoreBoardVR.GetComponent<ScoreBoardManager>();
        if (manager != null)
        {
            string type = isPedestrianSimulation ? "pedestrian" : "cyclist";
            manager.RefreshScoreboard(type, isFinalSim);
        }
    }
}