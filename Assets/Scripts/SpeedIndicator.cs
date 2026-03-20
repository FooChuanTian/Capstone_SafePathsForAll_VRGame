using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedIndicator : MonoBehaviour
{
    public TextMeshProUGUI SpeedText;
    public Rigidbody rb;
    public Transform tf;
    private Vector3 PreviousPosition;
    private Vector3 CurrentPosition;
    private Vector3 PositionDelta;
    private float SmoothSpeed;
    void Start()
    {
        PreviousPosition = tf.position;
        SmoothSpeed = 0;
    }
    void Update()
    {
        PositionDelta = rb.linearVelocity;
        PositionDelta = tf.position - PreviousPosition;
        float rawspeed = PositionDelta.magnitude / Time.deltaTime;
        SmoothSpeed = Mathf.Lerp(SmoothSpeed, rawspeed, Time.deltaTime*2f);
        SpeedText.text = "Speed: " + Math.Round(SmoothSpeed);
        //SpeedText.text = "Speed: " + (int) ((CurrentPosition - PreviousPosition).magnitude / Time.deltaTime);
        PreviousPosition = tf.position;
    }
}
