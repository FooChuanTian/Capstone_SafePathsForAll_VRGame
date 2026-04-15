using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;

public class ScoreDataManager : MonoBehaviour
{
    public static ScoreDataManager Instance;

    private string pedestrianCSVPath;
    private string cyclistCSVPath;
    private string currentPedestrianID;
    private string currentCyclistID;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitialiseCSVFiles();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitialiseCSVFiles()
    {
        // CRITICAL FIX: Use persistentDataPath instead of dataPath
        // persistentDataPath is ALWAYS writable on all platforms (Editor, Build, VR)
        string customPath = Path.Combine(Application.persistentDataPath, "Results");

        // Create the Results folder if it doesn't exist
        if (!Directory.Exists(customPath))
        {
            Directory.CreateDirectory(customPath);
            Debug.Log("[ScoreDataManager] Created Results folder at: " + customPath);
        }

        pedestrianCSVPath = Path.Combine(customPath, "pedestrian_scores.csv");
        cyclistCSVPath = Path.Combine(customPath, "cyclist_scores.csv");

        Debug.Log("[ScoreDataManager] CSV files will be saved to: " + customPath);
        Debug.Log("[ScoreDataManager] Pedestrian CSV: " + pedestrianCSVPath);
        Debug.Log("[ScoreDataManager] Cyclist CSV: " + cyclistCSVPath);

        // Create pedestrian CSV with headers if it doesn't exist
        if (!File.Exists(pedestrianCSVPath))
        {
            File.WriteAllText(pedestrianCSVPath,
                "PlayerID,FirstTotalScore,FirstLivesLeft,FirstSecondsCorrect,FirstSecondsWrong,FirstPhoneOpened,FirstTimestamp,LastTotalScore,LastLivesLeft,LastSecondsCorrect,LastSecondsWrong,LastPhoneOpened,LastTimestamp\n");
            Debug.Log("[ScoreDataManager] Created pedestrian_scores.csv");
        }

        // Create cyclist CSV with headers if it doesn't exist
        if (!File.Exists(cyclistCSVPath))
        {
            File.WriteAllText(cyclistCSVPath,
                "PlayerID,FirstTotalScore,FirstLivesLeft,FirstSecondsCorrect,FirstSecondsWrong,FirstTimeToSlow,FirstTimeToStop,FirstSpeedPenalty,FirstTimestamp,LastTotalScore,LastLivesLeft,LastSecondsCorrect,LastSecondsWrong,LastTimeToSlow,LastTimeToStop,LastSpeedPenalty,LastTimestamp\n");
            Debug.Log("[ScoreDataManager] Created cyclist_scores.csv");
        }
    }

    private string GeneratePedestrianID()
    {
        int nextPedID = PlayerPrefs.GetInt("NextPedestrianID", 1);
        string newID = "P_" + nextPedID.ToString("D4");
        PlayerPrefs.SetInt("NextPedestrianID", nextPedID + 1);
        PlayerPrefs.Save();
        Debug.Log("[ScoreDataManager] Generated Pedestrian ID: " + newID);
        return newID;
    }

    private string GenerateCyclistID()
    {
        int nextCycID = PlayerPrefs.GetInt("NextCyclistID", 1);
        string newID = "C_" + nextCycID.ToString("D4");
        PlayerPrefs.SetInt("NextCyclistID", nextCycID + 1);
        PlayerPrefs.Save();
        Debug.Log("[ScoreDataManager] Generated Cyclist ID: " + newID);
        return newID;
    }

    public string GetPlayerID()
    {
        if (!string.IsNullOrEmpty(currentPedestrianID))
            return currentPedestrianID;
        if (!string.IsNullOrEmpty(currentCyclistID))
            return currentCyclistID;
        return "UNKNOWN";
    }

    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrEmpty(currentPedestrianID))
            currentPedestrianID = name;
        if (!string.IsNullOrEmpty(currentCyclistID))
            currentCyclistID = name;
    }

    public void SavePedestrianScore(int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, int phoneOpened, bool isFirstSim)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Debug.Log($"[ScoreDataManager] SavePedestrianScore called - isFirstSim={isFirstSim}, totalScore={totalScore}");

        if (isFirstSim)
        {
            // Generate new pedestrian ID ONLY when first simulation starts
            currentPedestrianID = GeneratePedestrianID();

            // First simulation - create new row with first data filled, last data empty
            string row = $"{currentPedestrianID},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{phoneOpened},{timestamp},,,,,,,\n";

            try
            {
                File.AppendAllText(pedestrianCSVPath, row);
                Debug.Log("[ScoreDataManager] Pedestrian FIRST score saved for ID: " + currentPedestrianID);
                Debug.Log("[ScoreDataManager] Row written: " + row);
            }
            catch (Exception e)
            {
                Debug.LogError("[ScoreDataManager] ERROR writing first pedestrian score: " + e.Message);
            }
        }
        else
        {
            // Last simulation - update existing row with last data
            Debug.Log("[ScoreDataManager] Updating existing row for ID: " + currentPedestrianID);
            UpdatePedestrianCSVRow(timestamp, totalScore, livesLeft, secondsCorrect, secondsWrong, phoneOpened);
        }

        // Update scoreboard
        string simRun = isFirstSim ? "first" : "last";
        UpdateScoreboard("pedestrian", currentPedestrianID, totalScore, timestamp, simRun);
    }

    public void SaveCyclistScore(int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, float timeToSlow, float timeToStop, int speedPenalty, bool isFirstSim)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Debug.Log($"[ScoreDataManager] SaveCyclistScore called - isFirstSim={isFirstSim}, totalScore={totalScore}");

        if (isFirstSim)
        {
            // Generate new cyclist ID ONLY when first simulation starts
            currentCyclistID = GenerateCyclistID();

            // First simulation - create new row with first data filled, last data empty
            string row = $"{currentCyclistID},{totalScore},{livesLeft},{secondsCorrect},{secondsWrong},{timeToSlow:F2},{timeToStop:F2},{speedPenalty},{timestamp},,,,,,,,\n";

            try
            {
                File.AppendAllText(cyclistCSVPath, row);
                Debug.Log("[ScoreDataManager] Cyclist FIRST score saved for ID: " + currentCyclistID);
                Debug.Log("[ScoreDataManager] Row written: " + row);
            }
            catch (Exception e)
            {
                Debug.LogError("[ScoreDataManager] ERROR writing first cyclist score: " + e.Message);
            }
        }
        else
        {
            // Last simulation - update existing row with last data
            Debug.Log("[ScoreDataManager] Updating existing row for ID: " + currentCyclistID);
            UpdateCyclistCSVRow(currentCyclistID, timestamp, totalScore, livesLeft, secondsCorrect, secondsWrong, timeToSlow, timeToStop, speedPenalty);
        }

        // Update scoreboard
        string simRun = isFirstSim ? "first" : "last";
        UpdateScoreboard("cyclist", currentCyclistID, totalScore, timestamp, simRun);
    }

    private void UpdatePedestrianCSVRow(string timestamp, int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, int phoneOpened)
    {
        try
        {
            // Read all lines
            string[] lines = File.ReadAllLines(pedestrianCSVPath);
            Debug.Log($"[ScoreDataManager] Read {lines.Length} lines from pedestrian CSV");

            bool rowFound = false;

            // Find the row with matching playerID
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == currentPedestrianID)
                {
                    Debug.Log($"[ScoreDataManager] Found row for {currentPedestrianID} at line {i}");

                    // Update the "Last" columns (indices 7-12)
                    fields[7] = totalScore.ToString();
                    fields[8] = livesLeft.ToString();
                    fields[9] = secondsCorrect.ToString();
                    fields[10] = secondsWrong.ToString();
                    fields[11] = phoneOpened.ToString();
                    fields[12] = timestamp;

                    lines[i] = string.Join(",", fields);
                    Debug.Log("[ScoreDataManager] Updated row: " + lines[i]);
                    rowFound = true;
                    break;
                }
            }

            if (!rowFound)
            {
                Debug.LogError($"[ScoreDataManager] Could not find row for {currentPedestrianID}!");
            }
            else
            {
                // Write back to file
                File.WriteAllLines(pedestrianCSVPath, lines);
                Debug.Log("[ScoreDataManager] Pedestrian LAST score updated successfully");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("[ScoreDataManager] ERROR updating pedestrian CSV: " + e.Message);
        }
    }

    private void UpdateCyclistCSVRow(string cyclistID, string timestamp, int totalScore, int livesLeft, int secondsCorrect, int secondsWrong, float timeToSlow, float timeToStop, int speedPenalty)
    {
        try
        {
            // Read all lines
            string[] lines = File.ReadAllLines(cyclistCSVPath);
            Debug.Log($"[ScoreDataManager] Read {lines.Length} lines from cyclist CSV");

            bool rowFound = false;

            // Find the row with matching cyclistID
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields[0] == cyclistID)
                {
                    Debug.Log($"[ScoreDataManager] Found row for {cyclistID} at line {i}");

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
                    Debug.Log("[ScoreDataManager] Updated row: " + lines[i]);
                    rowFound = true;
                    break;
                }
            }

            if (!rowFound)
            {
                Debug.LogError($"[ScoreDataManager] Could not find row for {cyclistID}!");
            }
            else
            {
                // Write back to file
                File.WriteAllLines(cyclistCSVPath, lines);
                Debug.Log("[ScoreDataManager] Cyclist LAST score updated successfully");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("[ScoreDataManager] ERROR updating cyclist CSV: " + e.Message);
        }
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

        Debug.Log($"[ScoreDataManager] Scoreboard updated for {type} - {id}: {score}");
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