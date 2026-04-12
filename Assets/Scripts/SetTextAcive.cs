using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SetTextAcive : MonoBehaviour
{
    public GameObject TextToSet;

    public void SetTextActive()
    {
        TextToSet.SetActive(true);
    }

    public void SetTextInactive()
    {
        TextToSet.SetActive(false);
    }
}
