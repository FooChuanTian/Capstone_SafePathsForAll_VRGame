using System.Collections;
using UnityEngine;

public class PhoneDistraction_Pedestrian : MonoBehaviour
{
    [Header("References")]
    public PhoneToggle phoneToggle;
    public GameObject npcPrefab;
    public Transform npcSpawnPoint_CP2;
    public Transform npcSpawnPoint_CP3;

    [Header("Settings")]
    public float dismissWindow_CP2 = 4f;
    public float dismissWindow_CP3 = 2f;
    public float delayBeforePopup_CP3 = 2f;
    public float npcSpeed = 20f;

    private bool isProcessing = false;

    public void TriggerIntroDistraction()
    {
        if (isProcessing) return;
        StartCoroutine(IntroDistractionSequence());
    }

    public void TriggerDangerDistraction(Transform spawnPoint)
    {
        if (isProcessing) return;
        StartCoroutine(DangerDistractionSequence(spawnPoint, dismissWindow_CP2, 0f));
    }

    public void TriggerCP3Distraction()
    {
        if (isProcessing) return;
        StartCoroutine(DangerDistractionSequence(npcSpawnPoint_CP3, dismissWindow_CP3, delayBeforePopup_CP3));
    }

    IEnumerator IntroDistractionSequence()
    {
        isProcessing = true;

        phoneToggle.PhoneImage.gameObject.SetActive(true);
        Debug.Log("[Distraction] Intro: Phone popped up! No danger this time.");

        float timer = 0f;
        while (timer < dismissWindow_CP2)
        {
            if (!phoneToggle.PhoneImage.gameObject.activeSelf)
            {
                Debug.Log("[Distraction] Intro: Phone dismissed — good habit!");
                isProcessing = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("[Distraction] Intro: Time ran out, but no danger this time. Stay alert!");
        phoneToggle.PhoneImage.gameObject.SetActive(false);
        isProcessing = false;
    }

    IEnumerator DangerDistractionSequence(Transform spawnPoint, float dismissWindow, float delay)
    {
        isProcessing = true;

        // Wait before popping up (CP3 uses a delay, CP2 uses 0)
        if (delay > 0f)
        {
            Debug.Log("[Distraction] Waiting " + delay + " seconds before popup...");
            yield return new WaitForSeconds(delay);
        }

        phoneToggle.PhoneImage.gameObject.SetActive(true);
        Debug.Log("[Distraction] Danger: Phone popped up! Dismiss it quickly!");

        float timer = 0f;
        while (timer < dismissWindow)
        {
            if (!phoneToggle.PhoneImage.gameObject.activeSelf)
            {
                Debug.Log("[Distraction] Danger: Phone dismissed in time! Safe.");
                isProcessing = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("[Distraction] Danger: Phone not dismissed! NPC incoming.");
        phoneToggle.PhoneImage.gameObject.SetActive(false);
        SpawnDangerNPC(spawnPoint);
        isProcessing = false;
    }

    void SpawnDangerNPC(Transform spawnPoint)
    {
        if (npcPrefab == null || spawnPoint == null) return;

        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        NPCWalker walker = npc.AddComponent<NPCWalker>();
        walker.direction = Vector3.right;
        walker.speed = npcSpeed;

        Debug.Log("[Distraction] Danger NPC spawned at " + spawnPoint.name);
    }
}