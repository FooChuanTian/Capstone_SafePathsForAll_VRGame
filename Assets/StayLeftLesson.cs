using UnityEngine;
using TMPro;

public class StayLeftLesson : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text instructionText;

    [Header("Messages")]
    public string correctMessage = "Good! Pedestrians should keep left.";
    public string wrongMessage = "Oops! Pedestrians should keep left on shared paths.";

    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    public void PlayerEnteredLeft()
    {
        instructionText.text = correctMessage;
        instructionText.color = correctColor;
    }

    public void PlayerEnteredRight()
    {
        instructionText.text = wrongMessage;
        instructionText.color = wrongColor;
    }
}
