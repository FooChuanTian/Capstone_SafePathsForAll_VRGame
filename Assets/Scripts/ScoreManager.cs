using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Oculus.Interaction.Locomotion;

public class ScoreManager : MonoBehaviour
{
    public GameObject player;
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
    private SpeedIndicator speedIndicator;
    private float lastCollisionTime = -1f;
    [SerializeField] private float cooldown = 0.25f;
    private float OverallSpeedLimit = 25f;
    private float YellowSpeedLimit = 10f;
    private float RedSpeedLimit = 6f;
    public float TimetoSlow = 0f;
    public float TimetoStop = 0f;
    private bool HitYellow = false;
    private bool HitRed = false;
    public int MaximumSpeedPenalty = 0;
    public float currentSpeed;
    public Oculus.Interaction.Locomotion.CharacterController myController;

    void Start()
    {
        speedIndicator = player.GetComponent<SpeedIndicator>();
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
        if (myController.IsGrounded)
        {   Debug.Log("FTEST1");
            RaycastHit hitInfo = myController.GroundHit;

            if (hitInfo.collider.CompareTag(CorrectLanes[0]) || hitInfo.collider.CompareTag(CorrectLanes[1]))
            {   Debug.Log("FTEST2");
                SecondsOnCorrectLane++;
                if (CorrectLaneDebug != null)
                    CorrectLaneDebug.text = "Correct Lane: " + SecondsOnCorrectLane;
            } else
            {   Debug.Log("FTEST3");
                SecondsOnWrongLane++;
                if (WrongLaneDebug != null)
                    WrongLaneDebug.text = "Wrong Lane: " + SecondsOnWrongLane;
            }
        }
        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        // {
        //     foreach (var lane in AllLanes)
        //     {
        //         if (hit.collider.CompareTag(lane))
        //         {
        //             CurrentLane = hit.collider.tag;
        //             break;
        //         }
        //     }
        // }

        // if (CurrentLane != null)
        // {   Debug.Log("FTEST1" + CurrentLane);
        //     if (CorrectLanes.Contains(CurrentLane))
        //     {
        //         SecondsOnCorrectLane++;
        //         if (CorrectLaneDebug != null)
        //             CorrectLaneDebug.text = "Correct Lane: " + SecondsOnCorrectLane;
        //     }
        //     else if (AllLanes.Contains(CurrentLane))
        //     {
        //         SecondsOnWrongLane++;
        //         if (WrongLaneDebug != null)
        //             WrongLaneDebug.text = "Wrong Lane: " + SecondsOnWrongLane;
        //     }
        //     CurrentLane = null;
        // }

        if (speedIndicator != null)
            currentSpeed = speedIndicator.SmoothSpeed;
        if (currentSpeed > OverallSpeedLimit)
        {
            MaximumSpeedPenalty += (int)(currentSpeed - OverallSpeedLimit);
        }
    }

    void Update()
    {   
        if (PhoneObject != null)
        {
            if (XRSettings.enabled)
            {
                if (OVRInput.GetDown(OVRInput.Button.Four))
                {
                    PhoneOpened++;
                }
            }
            else
            {
                if (Keyboard.current.xKey.wasPressedThisFrame && PhoneObject.activeSelf)
                {
                    PhoneOpened++;   
                }
            }
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("speedtrackerSlow"))
        {
            if (Time.time - lastCollisionTime > cooldown)
            {
                if (speedIndicator.SmoothSpeed > YellowSpeedLimit)
                {
                    if (HitYellow) {
                        TimetoSlow += cooldown;
                    }
                    else
                    {
                        HitYellow = true;
                    }
                }
                else
                {
                    HitYellow = false;
                }
                lastCollisionTime = Time.time;
            }
        }
        else if (other.CompareTag("speedtrackerStop"))
        {
            if (Time.time - lastCollisionTime > cooldown)
            {
                if (speedIndicator.SmoothSpeed > RedSpeedLimit)
                {
                    if (HitRed) {
                        TimetoStop += cooldown;
                    }
                    else
                    {
                        HitRed = true;
                    }
                }
                else
                {
                    HitRed = false;
                }
                lastCollisionTime = Time.time;
            }
        }
    } 
}
