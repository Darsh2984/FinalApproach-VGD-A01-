
using UnityEngine;
using Unity.Cinemachine;

public class CameraDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private ThrowController throwController;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera javelinCamera;

    [Header("Settings")]
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 0;

    private Transform activeJavelin;

    private void OnEnable()
    {
        if (throwController != null)
        {
            throwController.OnJavelinLaunched += FollowJavelin;
            throwController.OnJavelinLanded += ShowLanding;
        }
    }

    private void OnDisable()
    {
        if (throwController != null)
        {
            throwController.OnJavelinLaunched -= FollowJavelin;
            throwController.OnJavelinLanded -= ShowLanding;
        }
    }

    private void Start()
    {
        ResetToPlayer();
    }

    public void FollowJavelin(Transform javelin)
    {
        if (javelin == null || javelinCamera == null)
            return;

        activeJavelin = javelin;

        javelinCamera.Target.TrackingTarget = activeJavelin;

        javelinCamera.Priority = activePriority;
        playerCamera.Priority = inactivePriority;
    }

    public void ShowLanding()
    {
        if (activeJavelin == null)
            return;

        // Keep the camera focused on the landed javelin.
        // We can add a separate landing camera later.
        javelinCamera.Priority = activePriority;
        playerCamera.Priority = inactivePriority;
    }

    public void ResetToPlayer()
    {
        activeJavelin = null;

        if (playerCamera == null || javelinCamera == null)
            return;

        playerCamera.Target.TrackingTarget = player;
        playerCamera.Priority = activePriority;

        javelinCamera.Priority = inactivePriority;
        javelinCamera.Target.TrackingTarget = null;
    }
}
