
using UnityEngine;
using TMPro;

public class LightSwitchInteraction : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Light[] roomLights;
    public GameObject interactionPrompt;
    public TMP_Text promptText;

    [Header("Settings")]
    public float interactionDistance = 2.5f;

    private bool lightsOn = true;

    void Start()
    {
        // Start with all room lights turned on.
        SetLights(true);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        bool isNearby = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isNearby);

        if (isNearby && promptText != null)
        {
            promptText.text = lightsOn
                ? "[E] Turn Lights Off"
                : "[E] Turn Lights On";
        }

        if (isNearby && Input.GetKeyDown(KeyCode.E))
        {
            lightsOn = !lightsOn;
            SetLights(lightsOn);
        }
    }

    void SetLights(bool enabled)
    {
        foreach (Light roomLight in roomLights)
        {
            if (roomLight != null)
                roomLight.enabled = enabled;
        }
    }
}
