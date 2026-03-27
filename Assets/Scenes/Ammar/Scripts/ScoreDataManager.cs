using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        // Generate pedestrian ID (will be used for pedestrian sessions)
        int nextPedID = PlayerPrefs.GetInt("NextPedestrianID", 1);
        playerID = "P_" + nextPedID.ToString("D4"); // P_0001, P_0002, etc.

        // Increment for next pedestrian session
        PlayerPrefs.SetInt("NextPedestrianID", nextPedID + 1);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[ScoreDataManager] Pedestrian ID initialized: " + playerID);
    }

    void InitialiseCSVFiles()
    {
        // Use Unity's dataPath which points to the Assets folder
        // Then append /Results subfolder
        string customPath = Path.Combine(UnityEngine.Application.dataPath, "Results");

        // Create the Results folder if it doesn't exist
        if (!Directory.Exists(customPath))
        {
            Directory.CreateDirectory(customPath);
            UnityEngine.Debug.Log("[ScoreDataManager] Created Results folder at: " + customPath);
        }

        pedestrianCSVPath = Path.Combine(customPath, "pedestrian_scores.csv");
        cyclistCSVPath = Path.Combine(customPath, "cyclist_scores.csv");

        UnityEngine.Debug.Log("[ScoreDataManager] CSV files location: " + customPath);

        // Create pedestrian CSV with headers if it doesn't exist
        if (!File.Exists(pedestrianCSVPath))
        {
            File.WriteAllText(pedestrianCSVPath,
                "PlayerID,FirstTotalScore,FirstLivesLeft,FirstSecondsCorrect,FirstSecondsWrong,FirstPhoneOpened,FirstTimestamp,LastTotalScore,LastLivesLeft,LastSecondsCorrect,LastSecondsWrong,LastPhoneOpened,LastTimestamp\n");
            UnityEngine.Debug.Log("[ScoreDataManager] Created pedestrian_scores.csv");
        }

        // Create cyclist CSV with headers if it doesn't exist
        if (!File.Exists(cyclistCSVPath))
        {
            File.WriteAllText(cyclistCSVPath,
                "PlayerID,FirstTotalScore,FirstLivesLeft,FirstSecondsCorrect,FirstSecondsWrong,FirstTimeToSlow,FirstTimeToStop,FirstSpeedPenalty,FirstTimestamp,LastTotalScore,LastLivesLeft,LastSecondsCorrect,LastSecondsWrong,LastTimeToSlow,LastTimeToStop,LastSpeedPenalty,LastTimestamp\n");
            UnityEngine.Debug.Log("[ScoreDataManager] Created cyclist_scores.csv");
        }
    }

    public string GetPlayerID()
    {
        return playerID;
    }

    public void SetPlayerName(string name)
    {
        playerID = name;
        PlayerPrefs.SetString("PlayerID", playerID);
        PlayerPrefs.Save();
    }

    public void SavePedestrianScore(int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, int phoneOpened, bool isFirstSim)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (isFirstSim)
        {
            // First simulation - create new row with first data filled, last data empty
            string row = $"{playerID},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{phoneOpened},{timestamp},,,,,,,\n";
            File.AppendAllText(pedestrianCSVPath, row);
            UnityEngine.Debug.Log("[ScoreDataManager] Pedestrian FIRST score saved for ID: " + playerID);
        }
        else
        {
            // Last simulation - update existing row with last data
            UpdatePedestrianCSVRow(timestamp, totalScore, livesLeft, secondsCorrect, secondsWrong, phoneOpened);
            UnityEngine.Debug.Log("[ScoreDataManager] Pedestrian LAST score updated for ID: " + playerID);
        }

        // Update scoreboard
        string simRun = isFirstSim ? "first" : "last";
        UpdateScoreboard("pedestrian", playerID, totalScore, timestamp, simRun);
    }

    public void SaveCyclistScore(int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, float timeToSlow, float timeToStop, int speedPenalty, bool isFirstSim)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string cyclistID;

        if (isFirstSim)
        {
            // Generate new cyclist ID for first simulation
            int nextCycID = PlayerPrefs.GetInt("NextCyclistID", 1);
            cyclistID = "C_" + nextCycID.ToString("D4");

            // Save this ID temporarily for the last simulation
            PlayerPrefs.SetString("CurrentCyclistID", cyclistID);

            // Increment for next cyclist
            PlayerPrefs.SetInt("NextCyclistID", nextCycID + 1);
            PlayerPrefs.Save();

            // First simulation - create new row with first data filled, last data empty
            string row = $"{cyclistID},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{timeToSlow:F2},{timeToStop:F2},{speedPenalty},{timestamp},,,,,,,,\n";
            File.AppendAllText(cyclistCSVPath, row);
            UnityEngine.Debug.Log("[ScoreDataManager] Cyclist FIRST score saved for ID: " + cyclistID);
        }
        else
        {
            // Use the same ID from first simulation
            cyclistID = PlayerPrefs.GetString("CurrentCyclistID", "C_0001");

            // Last simulation - update existing row with last data
            UpdateCyclistCSVRow(cyclistID, timestamp, totalScore, livesLeft, secondsCorrect, secondsWrong, timeToSlow, timeToStop, speedPenalty);
            UnityEngine.Debug.Log("[ScoreDataManager] Cyclist LAST score updated for ID: " + cyclistID);
        }

        // Update scoreboard
        string simRun = isFirstSim ? "first" : "last";
        UpdateScoreboard("cyclist", cyclistID, totalScore, timestamp, simRun);
    }

    private void UpdatePedestrianCSVRow(string timestamp, int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, int phoneOpened)
    {
        // Read all lines
        string[] lines = File.ReadAllLines(pedestrianCSVPath);

        // Find the row with matching playerID
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');
            if (fields[0] == playerID)
            {
                // Update the "Last" columns (indices 7-12)
                fields[7] = totalScore.ToString();
                fields[8] = livesLeft.ToString();
                fields[9] = secondsCorrect.ToString();
                fields[10] = secondsWrong.ToString();
                fields[11] = phoneOpened.ToString();
                fields[12] = timestamp;

                lines[i] = string.Join(",", fields);
                break;
            }
        }

        // Write back to file
        File.WriteAllLines(pedestrianCSVPath, lines);
    }

    private void UpdateCyclistCSVRow(string cyclistID, string timestamp, int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, float timeToSlow, float timeToStop, int speedPenalty)
    {
        // Read all lines
        string[] lines = File.ReadAllLines(cyclistCSVPath);

        // Find the row with matching cyclistID
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');
            if (fields[0] == cyclistID)
            {
                // Update the "Last" columns (indices 9-16)
                fields[9] = totalScore.ToString();
                fields[10] = livesLeft.ToString();
                fields[11] = secondsCorrect.ToString();
                fields[12] = secondsWrong.ToString();
                fields[13] = timeToSlow.ToString("F2");
                fields[14] = timeToStop.ToString("F2");
                fields[15] = speedPenalty.ToString();
                fields[16] = timestamp;

                lines[i] = string.Join(",", fields);
                break;
            }
        }

        // Write back to file
        File.WriteAllLines(cyclistCSVPath, lines);
    }

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

        entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (entries.Count > 20)
            entries = entries.GetRange(0, 20);

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
        List<ScoreEntry> entries = LoadScoreboard("pedestrian_scoreboard");

        return entries
            .Where(s => s.simRun == "last")
            .OrderByDescending(s => s.score)
            .Take(20)
            .ToList();
    }

    public List<ScoreEntry> GetCyclistScoreboard()
    {
        List<ScoreEntry> entries = LoadScoreboard("cyclist_scoreboard");

        return entries
            .Where(s => s.simRun == "last")
            .OrderByDescending(s => s.score)
            .Take(20)
            .ToList();
    }

    [System.Serializable]
    private class ScoreboardWrapper
    {
        public List<ScoreEntry> entries;
    }
}