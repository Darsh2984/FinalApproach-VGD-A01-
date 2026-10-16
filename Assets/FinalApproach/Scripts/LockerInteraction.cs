
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.Cinemachine;

public class LockerInteraction : MonoBehaviour
{
    [Header("References")]
    public Transform doorPivot;
    public Transform player;
    public GameObject interactionPrompt;
    public TMP_Text promptText;

    [Header("Camera Settings")]
    public CinemachineCamera lockerCamera;
    public CinemachineCamera playerCamera;
    public float cameraDuration = 2f;

    [Header("Interaction Settings")]
    public float interactionDistance = 2.5f;
    public float openAngle = 90f;
    public float rotationSpeed = 180f;

    private bool isOpen = false;
    private bool isShowingCloseUp = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (doorPivot == null)
        {
            Debug.LogError("LockerInteraction: Door Pivot is missing!");
            enabled = false;
            return;
        }

        closedRotation = doorPivot.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (lockerCamera != null)
            lockerCamera.Priority = 0;
    }

    void Update()
    {
        if (player == null || doorPivot == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            doorPivot.position
        );

        bool isNearby = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isNearby && !isShowingCloseUp);

        if (promptText != null)
            promptText.text = isOpen ? "[E] Close Locker" : "[E] Open Locker";

        if (isNearby &&
            !isShowingCloseUp &&
            Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;

            if (lockerCamera != null && playerCamera != null)
                StartCoroutine(ShowLockerCamera());
        }

        Quaternion targetRotation = isOpen
            ? openRotation
            : closedRotation;

        doorPivot.localRotation = Quaternion.RotateTowards(
            doorPivot.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    IEnumerator ShowLockerCamera()
    {
        isShowingCloseUp = true;

        lockerCamera.Priority = 20;
        playerCamera.Priority = 10;

        yield return new WaitForSeconds(cameraDuration);

        lockerCamera.Priority = 0;
        playerCamera.Priority = 10;

        isShowingCloseUp = false;
    }
}
