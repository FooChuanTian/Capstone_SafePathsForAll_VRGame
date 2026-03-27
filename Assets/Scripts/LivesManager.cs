using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public Transform FilledHeart1;
    public Transform FilledHeart2;
    public Transform FilledHeart3;
    public Transform EmptyHeart1;
    public Transform EmptyHeart2;
    public Transform EmptyHeart3;
    public int NumLives;

    void Start()
    {
        NumLives = 3;
        UpdateHearts(3);
    }
    public void UpdateHearts(int livesLeft)
    {
        switch (livesLeft)
        {
            case 0:
                FilledHeart1.gameObject.SetActive(false);
                FilledHeart2.gameObject.SetActive(false);
                FilledHeart3.gameObject.SetActive(false);
                EmptyHeart1.gameObject.SetActive(true);
                EmptyHeart2.gameObject.SetActive(true);
                EmptyHeart3.gameObject.SetActive(true);
                break;
            case 1:
                FilledHeart1.gameObject.SetActive(false);
                FilledHeart2.gameObject.SetActive(false);
                FilledHeart3.gameObject.SetActive(true);
                EmptyHeart1.gameObject.SetActive(true);
                EmptyHeart2.gameObject.SetActive(true);
                EmptyHeart3.gameObject.SetActive(false);
                break;
            case 2:
                FilledHeart1.gameObject.SetActive(false);
                FilledHeart2.gameObject.SetActive(true);
                FilledHeart3.gameObject.SetActive(true);
                EmptyHeart1.gameObject.SetActive(true);
                EmptyHeart2.gameObject.SetActive(false);
                EmptyHeart3.gameObject.SetActive(false);
                break;
            case 3:
                FilledHeart1.gameObject.SetActive(true);
                FilledHeart2.gameObject.SetActive(true);
                FilledHeart3.gameObject.SetActive(true);
                EmptyHeart1.gameObject.SetActive(false);
                EmptyHeart2.gameObject.SetActive(false);
                EmptyHeart3.gameObject.SetActive(false);
                break;
        }
    }
}
