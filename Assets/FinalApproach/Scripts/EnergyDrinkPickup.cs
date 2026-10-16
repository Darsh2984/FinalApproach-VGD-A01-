
using System.Collections;
using UnityEngine;
using TMPro;

public class EnergyDrinkPickup : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PlayerStamina playerStamina;
    public GameObject energyDrinkModel;
    public GameObject interactionPrompt;
    public TMP_Text promptText;

    [Header("Settings")]
    public float interactionDistance = 2.5f;
    public float respawnTime = 15f;

    private bool isAvailable = true;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (player == null || playerStamina == null ||
            energyDrinkModel == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            energyDrinkModel.transform.position
        );

        bool canInteract = isAvailable &&
                           distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(canInteract);

        if (canInteract && promptText != null)
            promptText.text = "[E] Drink Energy";

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            playerStamina.RestoreFullEnergy();

            isAvailable = false;
            energyDrinkModel.SetActive(false);

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            StartCoroutine(RespawnDrink());
        }
    }

    IEnumerator RespawnDrink()
    {
        yield return new WaitForSeconds(respawnTime);

        energyDrinkModel.SetActive(true);
        isAvailable = true;
    }
}
