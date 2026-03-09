using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnClick : MonoBehaviour
{
    void Start()
    {
        // Automatically link to the surviving manager
        GetComponent<Button>().onClick.AddListener(() => {
            if (ButtonSound.instance != null) {
                ButtonSound.instance.PlayClick();
            }
        });
    }
}