using System;
using TMPro;
using UnityEngine;

public class SpeedIndicator : MonoBehaviour
{
    public TextMeshProUGUI SpeedText;
    public Rigidbody rb;
void Update()
    {
        float velocity = rb.linearVelocity.magnitude;
        SpeedText.text = "Speed: " + (int) velocity;
    }
}
