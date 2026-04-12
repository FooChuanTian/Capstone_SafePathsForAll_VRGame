using Unity.VisualScripting;
using UnityEngine;

public class PlayerPositionManager_Ats : MonoBehaviour
{
    public Transform PreviousCheckpoint;
    public Transform Player;
    public static bool isPositionChanged = false;   //static variables stay even after scene is reloaded
    public static Vector3 PreviousPosition;

    public void Teleport()
    {
        Player.transform.position = PreviousCheckpoint.transform.position;
    }

    public void ChangePreviousCheckpoint(Transform NewCheckpoint)
    {
        PreviousCheckpoint = NewCheckpoint;
    }

    public void UpdatePreviousPosition(Transform checkpointLocation)
    {
        PreviousPosition = checkpointLocation.position;
        isPositionChanged = true;
    }

    public void ResetPositionChangeFlag()
    {
        isPositionChanged = false;
    }

    void Awake()
    {
        // Hitting checkpoints trigger isPositionChanged to true. When scene reloads, it runs this can autoupdates and teleports the player to the last checkpoint position
        if (isPositionChanged)
        {
            Player.position = PreviousPosition;
        }
    }
}
