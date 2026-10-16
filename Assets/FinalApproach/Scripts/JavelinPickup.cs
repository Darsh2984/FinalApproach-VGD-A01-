
using UnityEngine;
using TMPro;

public class JavelinPickup : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject equipmentJavelin;
    public GameObject interactionPrompt;
    public TMP_Text promptText;

    [Header("Settings")]
    public float interactionDistance = 2.5f;

    private bool hasPickedUp = false;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (player == null || equipmentJavelin == null)
            return;

        if (hasPickedUp)
            return;

        float distance = Vector3.Distance(
            player.position,
            equipmentJavelin.transform.position
        );

        bool isNearby = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isNearby);

        if (isNearby && promptText != null)
            promptText.text = "[E] Pick Up Javelin";

        if (isNearby && Input.GetKeyDown(KeyCode.E))
        {
            hasPickedUp = true;
            equipmentJavelin.SetActive(false);

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            Debug.Log("Javelin collected!");
        }
    }
}
