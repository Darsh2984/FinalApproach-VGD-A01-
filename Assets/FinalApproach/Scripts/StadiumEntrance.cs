
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StadiumEntrance : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject interactionPrompt;
    public TMP_Text promptText;

    [Header("Settings")]
    public float interactionDistance = 2.5f;
    public string stadiumSceneName = "FinalApproach_Stadium";

    void Start()
    {
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
            promptText.text = "[E] Enter Stadium";

        if (isNearby && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(stadiumSceneName);
        }
    }
}
