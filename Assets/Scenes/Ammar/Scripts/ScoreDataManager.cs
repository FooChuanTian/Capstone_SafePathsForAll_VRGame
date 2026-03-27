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
        // Always generate a new incremental ID for each game session
        int nextID = PlayerPrefs.GetInt("NextPlayerID", 1);
        playerID = nextID.ToString("D4"); // D4 = 4 digits with leading zeros (0001, 0002, etc.)

        // Increment for next session
        PlayerPrefs.SetInt("NextPlayerID", nextID + 1);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[ScoreDataManager] Player ID: " + playerID);
    }

    void InitialiseCSVFiles()
    {
        // Custom path - change this to your preferred location
        string customPath = @"C:\Documents\Github\Capstone_SafePathsForAll_VRGame\Assets\Results";

        // Create the Results folder if it doesn't exist
        if (!Directory.Exists(customPath))
        {
            Directory.CreateDirectory(customPath);
            UnityEngine.Debug.Log("[ScoreDataManager] Created Results folder at: " + customPath);
        }

        pedestrianCSVPath = Path.Combine(customPath, "pedestrian_scores.csv");
        cyclistCSVPath = Path.Combine(customPath, "cyclist_scores.csv");

        // Create pedestrian CSV with headers if it doesn't exist
        if (!File.Exists(pedestrianCSVPath))
        {
            File.WriteAllText(pedestrianCSVPath,
                "PlayerID,FirstTotalScore,FirstLivesLeft,FirstSecondsCorrect,FirstSecondsWrong,FirstPhoneOpened,FirstTimestamp,LastTotalScore,LastLivesLeft,LastSecondsCorrect,LastSecondsWrong,LastPhoneOpened,LastTimestamp\n");
            UnityEngine.Debug.Log("[ScoreDataManager] Created pedestrian_scores.csv at: " + pedestrianCSVPath);
        }

        // Create cyclist CSV with headers if it doesn't exist
        if (!File.Exists(cyclistCSVPath))
        {
            File.WriteAllText(cyclistCSVPath,
                "PlayerID,FirstTotalScore,FirstLivesLeft,FirstSecondsCorrect,FirstSecondsWrong,FirstTimeToSlow,FirstTimeToStop,FirstSpeedPenalty,FirstTimestamp,LastTotalScore,LastLivesLeft,LastSecondsCorrect,LastSecondsWrong,LastTimeToSlow,LastTimeToStop,LastSpeedPenalty,LastTimestamp\n");
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
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (isFirstSim)
        {
            // First simulation - create new row with first data filled, last data empty
            string row = $"{playerID},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{phoneOpened},{timestamp},,,,,,,\n";
            File.AppendAllText(pedestrianCSVPath, row);
            UnityEngine.Debug.Log("[ScoreDataManager] Pedestrian FIRST score saved: " + row);
        }
        else
        {
            // Last simulation - update existing row with last data
            UpdatePedestrianCSVRow(timestamp, totalScore, livesLeft, secondsCorrect, secondsWrong, phoneOpened);
            UnityEngine.Debug.Log("[ScoreDataManager] Pedestrian LAST score updated for player: " + playerID);
        }

        // Update scoreboard
        string simRun = isFirstSim ? "first" : "last";
        UpdateScoreboard("pedestrian", playerID, totalScore, timestamp, simRun);
    }

    public void SaveCyclistScore(int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, float timeToSlow, float timeToStop, int speedPenalty, bool isFirstSim)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (isFirstSim)
        {
            // First simulation - create new row with first data filled, last data empty
            string row = $"{playerID},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{timeToSlow:F2},{timeToStop:F2},{speedPenalty},{timestamp},,,,,,,,\n";
            File.AppendAllText(cyclistCSVPath, row);
            UnityEngine.Debug.Log("[ScoreDataManager] Cyclist FIRST score saved: " + row);
        }
        else
        {
            // Last simulation - update existing row with last data
            UpdateCyclistCSVRow(timestamp, totalScore, livesLeft, secondsCorrect, secondsWrong, timeToSlow, timeToStop, speedPenalty);
            UnityEngine.Debug.Log("[ScoreDataManager] Cyclist LAST score updated for player: " + playerID);
        }

        // Update scoreboard
        string simRun = isFirstSim ? "first" : "last";
        UpdateScoreboard("cyclist", playerID, totalScore, timestamp, simRun);
    }

    private void UpdatePedestrianCSVRow(string timestamp, int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, int phoneOpened)
    {
        // Read all lines
        string[] lines = File.ReadAllLines(pedestrianCSVPath);

        // Find the row with matching playerID
        for (int i = 1; i < lines.Length; i++) // Start at 1 to skip header
        {
            string[] fields = lines[i].Split(',');
            if (fields[0] == playerID)
            {
                // Update the "Last" columns (indices 7-12)
                fields[7] = totalScore.ToString();     // LastTotalScore
                fields[8] = livesLeft.ToString();      // LastLivesLeft
                fields[9] = secondsCorrect.ToString(); // LastSecondsCorrect
                fields[10] = secondsWrong.ToString();  // LastSecondsWrong
                fields[11] = phoneOpened.ToString();   // LastPhoneOpened
                fields[12] = timestamp;                // LastTimestamp

                lines[i] = string.Join(",", fields);
                break;
            }
        }

        // Write back to file
        File.WriteAllLines(pedestrianCSVPath, lines);
    }

    private void UpdateCyclistCSVRow(string timestamp, int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, float timeToSlow, float timeToStop, int speedPenalty)
    {
        // Read all lines
        string[] lines = File.ReadAllLines(cyclistCSVPath);

        // Find the row with matching playerID
        for (int i = 1; i < lines.Length; i++) // Start at 1 to skip header
        {
            string[] fields = lines[i].Split(',');
            if (fields[0] == playerID)
            {
                // Update the "Last" columns (indices 9-16)
                fields[9] = totalScore.ToString();         // LastTotalScore
                fields[10] = livesLeft.ToString();         // LastLivesLeft
                fields[11] = secondsCorrect.ToString();    // LastSecondsCorrect
                fields[12] = secondsWrong.ToString();      // LastSecondsWrong
                fields[13] = timeToSlow.ToString("F2");    // LastTimeToSlow
                fields[14] = timeToStop.ToString("F2");    // LastTimeToStop
                fields[15] = speedPenalty.ToString();      // LastSpeedPenalty
                fields[16] = timestamp;                    // LastTimestamp

                lines[i] = string.Join(",", fields);
                break;
            }
        }

        // Write back to file
        File.WriteAllLines(cyclistCSVPath, lines);
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
        List<ScoreEntry> entries = LoadScoreboard("pedestrian_scoreboard");

        // Filter for only "last" simulation runs
        return entries
            .Where(s => s.simRun == "last")
            .OrderByDescending(s => s.score)
            .Take(20)
            .ToList();
    }

    public List<ScoreEntry> GetCyclistScoreboard()
    {
        List<ScoreEntry> entries = LoadScoreboard("cyclist_scoreboard");

        // Filter for only "last" simulation runs
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