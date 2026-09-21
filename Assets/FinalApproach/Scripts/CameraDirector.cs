using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    // ========================================
    // REFERENCES
    // ========================================

    [Header("References")]

    [SerializeField]
    private Transform player;

    [SerializeField]
    private ThrowController throwController;


    // ========================================
    // CAMERA OFFSETS
    // ========================================

    [Header("Camera Offsets")]

    [SerializeField]
    private Vector3 playerOffset =
        new Vector3(10f, 5f, -8f);

    [SerializeField]
    private Vector3 javelinOffset =
        new Vector3(9f, 4f, -12f);

    [SerializeField]
    private Vector3 landingOffset =
        new Vector3(9f, 5f, -12f);


    // ========================================
    // CAMERA SMOOTHING
    // ========================================

    [Header("Camera Smoothing")]

    [SerializeField]
    private float playerSmoothTime = 0.30f;

    [SerializeField]
    private float javelinSmoothTime = 0.20f;

    [SerializeField]
    private float landingSmoothTime = 0.40f;

    [SerializeField]
    private float rotationSpeed = 6f;


    // ========================================
    // CAMERA STATES
    // ========================================

    private enum CameraState
    {
        FollowPlayer,
        FollowJavelin,
        ShowLanding
    }

    private CameraState currentState =
        CameraState.FollowPlayer;


    // ========================================
    // INTERNAL VARIABLES
    // ========================================

    private Transform activeJavelin;

    private Vector3 landingPosition;

    private Vector3 cameraVelocity;


    // ========================================
    // EVENT SUBSCRIPTIONS
    // ========================================

    private void OnEnable()
    {
        if (throwController != null)
        {
            throwController.OnJavelinLaunched +=
                FollowJavelin;

            throwController.OnJavelinLanded +=
                ShowLanding;
        }
    }


    private void OnDisable()
    {
        if (throwController != null)
        {
            throwController.OnJavelinLaunched -=
                FollowJavelin;

            throwController.OnJavelinLanded -=
                ShowLanding;
        }
    }


    // ========================================
    // INITIAL CAMERA
    // ========================================

    private void Start()
    {
        ResetToPlayer();
    }


    // ========================================
    // CAMERA UPDATE
    // ========================================

    private void LateUpdate()
    {
        switch (currentState)
        {
            case CameraState.FollowPlayer:

                UpdatePlayerCamera();

                break;


            case CameraState.FollowJavelin:

                UpdateJavelinCamera();

                break;


            case CameraState.ShowLanding:

                UpdateLandingCamera();

                break;
        }
    }


    // ========================================
    // FOLLOW PLAYER
    // ========================================

    private void UpdatePlayerCamera()
    {
        if (player == null)
        {
            return;
        }

        Vector3 targetPosition =
            player.position + playerOffset;

        MoveCamera(
            targetPosition,
            player.position + Vector3.up,
            playerSmoothTime
        );
    }


    // ========================================
    // FOLLOW JAVELIN
    // ========================================

    private void UpdateJavelinCamera()
    {
        if (activeJavelin == null)
        {
            return;
        }

        Vector3 targetPosition =
            activeJavelin.position +
            javelinOffset;

        MoveCamera(
            targetPosition,
            activeJavelin.position,
            javelinSmoothTime
        );
    }


    // ========================================
    // SHOW LANDING
    // ========================================

    private void UpdateLandingCamera()
    {
        Vector3 targetPosition =
            landingPosition +
            landingOffset;

        MoveCamera(
            targetPosition,
            landingPosition + Vector3.up,
            landingSmoothTime
        );
    }


    // ========================================
    // SMOOTH CAMERA MOVEMENT
    // ========================================

    private void MoveCamera(
        Vector3 targetPosition,
        Vector3 lookTarget,
        float smoothTime
    )
    {
        transform.position =
            Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref cameraVelocity,
                smoothTime
            );


        Vector3 direction =
            lookTarget - transform.position;


        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    direction
                );


            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed *
                    Time.deltaTime
                );
        }
    }


    // ========================================
    // SWITCH TO JAVELIN
    // ========================================

    public void FollowJavelin(
        Transform javelin
    )
    {
        activeJavelin = javelin;

        cameraVelocity = Vector3.zero;

        currentState =
            CameraState.FollowJavelin;
    }


    // ========================================
    // SWITCH TO LANDING
    // ========================================

    public void ShowLanding()
    {
        if (activeJavelin == null)
        {
            return;
        }

        landingPosition =
            activeJavelin.position;

        cameraVelocity = Vector3.zero;

        currentState =
            CameraState.ShowLanding;
    }


    // ========================================
    // RESET CAMERA
    // ========================================

    public void ResetToPlayer()
    {
        currentState =
            CameraState.FollowPlayer;

        activeJavelin = null;

        cameraVelocity = Vector3.zero;


        if (player == null)
        {
            return;
        }


        transform.position =
            player.position + playerOffset;


        Vector3 lookTarget =
            player.position + Vector3.up;


        transform.LookAt(
            lookTarget
        );
    }
}