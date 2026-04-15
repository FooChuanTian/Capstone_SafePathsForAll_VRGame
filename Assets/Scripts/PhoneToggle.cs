using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PhoneToggle : MonoBehaviour
{
    public Transform PhoneImage;
    private bool phoneActive;
    public Image targetImage;
    private Sprite[] backgroundSprites;
    private const string folderPath = "Screenshots"; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhoneImage.gameObject.SetActive(false);
        phoneActive = false;
        backgroundSprites = Resources.LoadAll<Sprite>(folderPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame || OVRInput.GetDown(OVRInput.Button.Four))
        {
            //DisplayRandomImage();
            phoneActive = !phoneActive;
            PhoneImage.gameObject.SetActive(phoneActive);
        }
    }

    public void DisplayRandomImage()
    {
        // Generate a random index within the array's bounds
        // Random.Range(minInclusive, maxExclusive)
        int randomIndex = Random.Range(0, backgroundSprites.Length);
        Debug.Log("Length of folder: " + backgroundSprites.Length);

        // Assign the random sprite to the UI Image component
        if (targetImage != null)
        {
            targetImage.sprite = backgroundSprites[randomIndex];
        }
        else
        {
            Debug.LogError("Target Image component not assigned!");
        }
    }
}
