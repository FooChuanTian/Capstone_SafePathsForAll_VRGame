using Debug = UnityEngine.Debug;
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

        // Uncomment and fix to use ScoreBoardVR:
        Transform scoreboardList = ScoreBoardVR.Find("ScoreboardList");
        if (scoreboardList != null)
            scoreboardList.gameObject.SetActive(isFinalSim);
        else
            Debug.LogError("[OpenScoreBoard] ScoreboardList not found under ScoreBoardVR!");

        // Refresh scoreboard entries now that it's visible
        ScoreBoardManager manager = ScoreBoardVR.GetComponent<ScoreBoardManager>();
        if (manager != null)
        {
            string type = isPedestrianSimulation ? "pedestrian" : "cyclist";
            manager.RefreshScoreboard(type, isFinalSim);
        }
    }
}