using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public static ButtonSound instance; // The "Master" version
    public AudioSource source;
    public AudioClip clickSound;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClick()
    {
        source.PlayOneShot(clickSound);
    }
}