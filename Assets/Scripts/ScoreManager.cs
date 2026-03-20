using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    public int SecondsOnCorrectLane;
    public int SecondsOnWrongLane;
    public int PhoneOpened;
    public string PlayerType;
    public TextMeshProUGUI CorrectLaneDebug;
    public TextMeshProUGUI WrongLaneDebug;
    public GameObject PhoneObject;
    private string[] CorrectLanes;
    private string CurrentLane;

    private string[] AllLanes = {"pedestrian_lane_left", "pedestrian_lane_right", "cycling_lane_left", "cycling_lane_right"};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SecondsOnCorrectLane = 0;
        SecondsOnWrongLane = 0;
        PhoneOpened = 0;

        if (PlayerType == "pedestrian")
        {
            CorrectLanes = new string[] {"pedestrian_lane_left", "pedestrian_lane_right"};

        }
        else if (PlayerType == "cyclist")
        {
            CorrectLanes = new string[] {"cycling_lane_left", "cycling_lane_right"};
        }
        InvokeRepeating(nameof(SecondUpdate), 0f, 1.0f);
    }

    void SecondUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var hitCollider in hitColliders) {
            if (AllLanes.Contains(hitCollider.gameObject.tag))
            {
                CurrentLane = hitCollider.gameObject.tag;
                break;
            }
        }
        if (CurrentLane != null)
        {
            if (CorrectLanes.Contains(CurrentLane))
            {
                SecondsOnCorrectLane++;
                CorrectLaneDebug.text = "Correct Lane: " + SecondsOnCorrectLane;
            }
            else
            {
                SecondsOnWrongLane++;
                WrongLaneDebug.text = "Wrong Lane: " + SecondsOnWrongLane;
            }
            CurrentLane = null;
        }
    }

    void Update()
    {
        if (PhoneObject != null)
        {
            if (Keyboard.current.xKey.wasPressedThisFrame && PhoneObject.activeSelf)
            {
                PhoneOpened++;   
            }
        }
    }
}
