using Unity.VisualScripting;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    public Transform PreviousCheckpoint;
    public Transform Player;

    public void Teleport()
    {
        Player.transform.position = PreviousCheckpoint.transform.position;
    }

    public void ChangePreviousCheckpoint(Transform NewCheckpoint)
    {
        PreviousCheckpoint = NewCheckpoint;
    }
}
