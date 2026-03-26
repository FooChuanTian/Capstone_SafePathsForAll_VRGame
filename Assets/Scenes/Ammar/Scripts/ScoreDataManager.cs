using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreDataManager : MonoBehaviour
{
    public static ScoreDataManager Instance;

    private string pedestrianCSVPath;
    private string cyclistCSVPath;
    private string playerID;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitialisePlayerID();
            InitialiseCSVFiles();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitialisePlayerID()
    {
        // Check if player already has an ID saved
        if (PlayerPrefs.HasKey("PlayerID"))
        {
            playerID = PlayerPrefs.GetString("PlayerID");
        }
        else
        {
            // Generate a new random ID
            playerID = "P_" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            PlayerPrefs.SetString("PlayerID", playerID);
            PlayerPrefs.Save();
        }
        UnityEngine.Debug.Log("[ScoreDataManager] Player ID: " + playerID);
    }

    void InitialiseCSVFiles()
    {
        pedestrianCSVPath = Path.Combine(UnityEngine.Application.persistentDataPath, "pedestrian_scores.csv");
        cyclistCSVPath = Path.Combine(UnityEngine.Application.persistentDataPath, "cyclist_scores.csv");

        // Create pedestrian CSV with headers if it doesn't exist
        if (!File.Exists(pedestrianCSVPath))
        {
            File.WriteAllText(pedestrianCSVPath,
                "PlayerID,Timestamp,SimulationRun,TotalScore,LivesLeft,SecondsOnCorrectLane,SecondsOnWrongLane,PhoneOpened\n");
            UnityEngine.Debug.Log("[ScoreDataManager] Created pedestrian_scores.csv at: " + pedestrianCSVPath);
        }

        // Create cyclist CSV with headers if it doesn't exist
        if (!File.Exists(cyclistCSVPath))
        {
            File.WriteAllText(cyclistCSVPath,
                "PlayerID,Timestamp,SimulationRun,TotalScore,LivesLeft,SecondsOnCorrectLane,SecondsOnWrongLane,TimeToSlow,TimeToStop,SpeedPenalty\n");
            UnityEngine.Debug.Log("[ScoreDataManager] Created cyclist_scores.csv at: " + cyclistCSVPath);
        }
    }

    public string GetPlayerID()
    {
        return playerID;
    }

    // Call this to update player ID to a real name later
    public void SetPlayerName(string name)
    {
        playerID = name;
        PlayerPrefs.SetString("PlayerID", playerID);
        PlayerPrefs.Save();
    }

    public void SavePedestrianScore(int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, int phoneOpened, bool isFirstSim)
    {
        string simRun = isFirstSim ? "first" : "last";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string row = $"{playerID},{timestamp},{simRun},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{phoneOpened}\n";

        File.AppendAllText(pedestrianCSVPath, row);
        UnityEngine.Debug.Log("[ScoreDataManager] Pedestrian score saved: " + row);

        // Update scoreboard
        UpdateScoreboard("pedestrian", playerID, totalScore, timestamp, simRun);
    }

    public void SaveCyclistScore(int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, float timeToSlow, float timeToStop, int speedPenalty, bool isFirstSim)
    {
        string simRun = isFirstSim ? "first" : "last";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string row = $"{playerID},{timestamp},{simRun},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{timeToSlow:F2},{timeToStop:F2},{speedPenalty}\n";

        File.AppendAllText(cyclistCSVPath, row);
        UnityEngine.Debug.Log("[ScoreDataManager] Cyclist score saved: " + row);

        // Update scoreboard
        UpdateScoreboard("cyclist", playerID, totalScore, timestamp, simRun);
    }

    // ── Scoreboard ──────────────────────────────────────────────────────────

    [System.Serializable]
    public class ScoreEntry
    {
        public string playerID;
        public int score;
        public string timestamp;
        public string simRun;
    }

    private void UpdateScoreboard(string type, string id, int score, string timestamp, string simRun)
    {
        string key = type + "_scoreboard";
        List<ScoreEntry> entries = LoadScoreboard(key);

        entries.Add(new ScoreEntry { playerID = id, score = score, timestamp = timestamp, simRun = simRun });

        // Sort descending and keep top 20
        entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (entries.Count > 20)
            entries = entries.GetRange(0, 20);

        // Save back to PlayerPrefs as JSON
        string json = JsonUtility.ToJson(new ScoreboardWrapper { entries = entries });
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[ScoreDataManager] Scoreboard updated for " + type);
    }

    public List<ScoreEntry> LoadScoreboard(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            ScoreboardWrapper wrapper = JsonUtility.FromJson<ScoreboardWrapper>(json);
            return wrapper.entries ?? new List<ScoreEntry>();
        }
        return new List<ScoreEntry>();
    }

    public List<ScoreEntry> GetPedestrianScoreboard()
    {
        return LoadScoreboard("pedestrian_scoreboard");
    }

    public List<ScoreEntry> GetCyclistScoreboard()
    {
        return LoadScoreboard("cyclist_scoreboard");
    }

    [System.Serializable]
    private class ScoreboardWrapper
    {
        public List<ScoreEntry> entries;
    }
}