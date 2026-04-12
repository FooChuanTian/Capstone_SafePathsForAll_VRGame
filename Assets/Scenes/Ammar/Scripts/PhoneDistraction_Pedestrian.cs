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

    [Header("CP3 Multi-Popup Gaps")]
    public float gapBetweenPopup1and2 = 3f;
    public float gapBetweenPopup2and3 = 2f;

    [Header("Manual Phone Penalty")]
    public float manualPhoneTimeLimit = 4f;
    public Transform manualPhoneSpawnPoint;

    private bool isProcessing = false;
    private bool manualPenaltyEnabled = false; // Only enabled after CP2 danger distraction
    private float manualPhoneTimer = 0f;

    void Update()
    {
        if (!isProcessing && manualPenaltyEnabled && phoneToggle.PhoneImage.gameObject.activeSelf)
        {
            manualPhoneTimer += Time.deltaTime;

            if (manualPhoneTimer >= manualPhoneTimeLimit)
            {
                manualPhoneTimer = 0f;
                phoneToggle.PhoneImage.gameObject.SetActive(false);
                SpawnDangerNPC(manualPhoneSpawnPoint != null ? manualPhoneSpawnPoint : npcSpawnPoint_CP2);
                UnityEngine.Debug.Log("[Distraction] Player kept phone open too long! NPC spawned.");
            }
        }
        else if (!phoneToggle.PhoneImage.gameObject.activeSelf)
        {
            manualPhoneTimer = 0f;
        }
    }

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
        StartCoroutine(CP3DistractionSequence());
    }

    IEnumerator IntroDistractionSequence()
    {
        isProcessing = true;

        phoneToggle.PhoneImage.gameObject.SetActive(true);
        UnityEngine.Debug.Log("[Distraction] Intro: Phone popped up!");

        float timer = 0f;
        while (timer < dismissWindow_CP2)
        {
            if (!phoneToggle.PhoneImage.gameObject.activeSelf)
            {
                UnityEngine.Debug.Log("[Distraction] Intro: Dismissed!");
                isProcessing = false;
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        phoneToggle.PhoneImage.gameObject.SetActive(false);
        UnityEngine.Debug.Log("[Distraction] Intro: Time ran out, no danger this time.");
        isProcessing = false;
    }

    IEnumerator DangerDistractionSequence(Transform spawnPoint, float dismissWindow, float delay)
    {
        isProcessing = true;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        phoneToggle.PhoneImage.gameObject.SetActive(true);
        UnityEngine.Debug.Log("[Distraction] Danger: Phone popped up!");

        float timer = 0f;
        while (timer < dismissWindow)
        {
            if (!phoneToggle.PhoneImage.gameObject.activeSelf)
            {
                UnityEngine.Debug.Log("[Distraction] Danger: Dismissed in time!");
                manualPenaltyEnabled = true; // Enable manual penalty after CP2 is dismissed
                isProcessing = false;
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        phoneToggle.PhoneImage.gameObject.SetActive(false);
        SpawnDangerNPC(spawnPoint);
        manualPenaltyEnabled = true; // Enable manual penalty after CP2 times out
        isProcessing = false;
    }

    IEnumerator CP3DistractionSequence()
    {
        isProcessing = true;

        yield return new WaitForSeconds(delayBeforePopup_CP3);

        yield return StartCoroutine(SinglePopup());
        yield return new WaitForSeconds(gapBetweenPopup1and2);

        yield return StartCoroutine(SinglePopup());
        yield return new WaitForSeconds(gapBetweenPopup2and3);

        yield return StartCoroutine(SinglePopup());

        isProcessing = false;
    }

    IEnumerator SinglePopup()
    {
        phoneToggle.PhoneImage.gameObject.SetActive(true);
        UnityEngine.Debug.Log("[Distraction] CP3: Phone popped up!");

        float timer = 0f;
        while (timer < dismissWindow_CP3)
        {
            if (!phoneToggle.PhoneImage.gameObject.activeSelf)
            {
                UnityEngine.Debug.Log("[Distraction] CP3: Dismissed in time!");
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        phoneToggle.PhoneImage.gameObject.SetActive(false);
        SpawnDangerNPC(npcSpawnPoint_CP3);
        UnityEngine.Debug.Log("[Distraction] CP3: Failed! NPC spawned.");
    }

    void SpawnDangerNPC(Transform spawnPoint)
    {
        if (npcPrefab == null || spawnPoint == null) return;

        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        NPCWalker walker = npc.AddComponent<NPCWalker>();
        walker.direction = Vector3.right;
        walker.speed = npcSpeed;
    }
}