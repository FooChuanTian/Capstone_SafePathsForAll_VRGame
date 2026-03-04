using UnityEngine;

public class LaneTrigger : MonoBehaviour
{
    public enum LaneType
    {
        PedestrianLeft,
        PedestrianRight
    }

    public LaneType laneType;
    public StayLeftLesson lesson;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (laneType == LaneType.PedestrianLeft)
        {
            lesson.PlayerEnteredLeft();
        }
        else if (laneType == LaneType.PedestrianRight)
        {
            lesson.PlayerEnteredRight();
        }
    }
}
