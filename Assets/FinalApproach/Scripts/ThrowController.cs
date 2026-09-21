using UnityEngine;
using System;

public class ThrowController : MonoBehaviour
{
    // ====================================
    // REFERENCES
    // ====================================

    [Header("References")]

    [SerializeField]
    private PlayerRunController runController;

    [SerializeField]
    private JavelinController javelinPrefab;

    [SerializeField]
    private Transform javelinHoldPoint;

    [SerializeField]
    private Collider landingFieldCollider;


    // ====================================
    // THROW SETTINGS
    // ====================================

    [Header("Throw Settings")]

    [SerializeField]
    private float baseThrowVelocity = 18f;

    [SerializeField]
    private float runContribution = 0.9f;


    // ====================================
    // ANGLE SETTINGS
    // ====================================

    [Header("Throw Angle")]

    [SerializeField]
    private float throwAngle = 35f;

    [SerializeField]
    private float minimumAngle = 15f;

    [SerializeField]
    private float maximumAngle = 60f;

    [SerializeField]
    private float angleChangeSpeed = 40f;


    // ====================================
    // THROW LINE
    // ====================================

    [Header("Throw Line")]

    [SerializeField]
    private float throwLineZ = 0f;


    // ====================================
    // INTERNAL VARIABLES
    // ====================================

    private bool isPreparing = false;

    private bool hasThrown = false;

    private JavelinController activeJavelin;

    private string releaseQualityLabel;


    // ====================================
    // RESULT EVENT
    // ====================================

    public event Action<bool, float, string>
        OnThrowCompleted;

    // Camera events

    public event Action<Transform>
        OnJavelinLaunched;

    public event Action
        OnJavelinLanded;


    // ====================================
    // UPDATE
    // ====================================

    private void Update()
    {
        if (hasThrown)
        {
            return;
        }

        HandlePreparation();

        HandleAngle();

        HandleRelease();
    }


    // ====================================
    // PREPARE THROW
    // ====================================

    private void HandlePreparation()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isPreparing = true;

            Debug.Log("Preparing throw...");
        }
    }


    // ====================================
    // CHANGE ANGLE
    // ====================================

    private void HandleAngle()
    {
        if (!isPreparing)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            throwAngle +=
                angleChangeSpeed *
                Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            throwAngle -=
                angleChangeSpeed *
                Time.deltaTime;
        }

        throwAngle = Mathf.Clamp(
            throwAngle,
            minimumAngle,
            maximumAngle
        );
    }


    // ====================================
    // RELEASE INPUT
    // ====================================

    private void HandleRelease()
    {
        if (!isPreparing)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ThrowJavelin();
        }
    }


    // ====================================
    // THROW JAVELIN
    // ====================================

    private void ThrowJavelin()
    {
        hasThrown = true;

        isPreparing = false;

        runController.StopRunning();


        float distanceToLine =
            throwLineZ - transform.position.z;


        // FOUL CHECK

        if (distanceToLine < 0f)
        {
            Debug.Log("FOUL!");

            OnThrowCompleted?.Invoke(
                false,
                0f,
                "FOUL"
            );

            return;
        }


        float releaseQuality =
            CalculateReleaseQuality(
                distanceToLine
            );


        releaseQualityLabel =
            GetReleaseQualityLabel(
                distanceToLine
            );


        float playerSpeed =
            runController.CurrentSpeed;


        float stability =
            CalculateStability(
                playerSpeed
            );


        float launchSpeed =
            (
                baseThrowVelocity +
                playerSpeed * runContribution
            )
            *
            releaseQuality
            *
            stability;


        float angleRadians =
            throwAngle * Mathf.Deg2Rad;


        float forwardVelocity =
            launchSpeed *
            Mathf.Cos(angleRadians);


        float upwardVelocity =
            launchSpeed *
            Mathf.Sin(angleRadians);


        Vector3 launchVelocity =
            new Vector3(
                0f,
                upwardVelocity,
                forwardVelocity
            );


        activeJavelin =
            Instantiate(
                javelinPrefab,
                javelinHoldPoint.position,
                Quaternion.Euler(
                    90f - throwAngle,
                    0f,
                    0f
                )
            );

        // Tell the camera to follow this javelin.

        OnJavelinLaunched?.Invoke(
            activeJavelin.transform
        );


        // Subscribe before launching so we
        // cannot miss the landing notification.

        activeJavelin.OnJavelinLanded +=
            HandleJavelinLanding;


        activeJavelin.Launch(
            launchVelocity,
            throwLineZ,
            landingFieldCollider
        );


        Debug.Log(
            "Release Quality: " +
            releaseQualityLabel
        );
    }


    // ====================================
    // HANDLE JAVELIN LANDING
    // ====================================

    private void HandleJavelinLanding(
    float distance
)
    {
        if (activeJavelin != null)
        {
            activeJavelin.OnJavelinLanded -=
                HandleJavelinLanding;
        }


        // Tell the camera the throw has landed.

        OnJavelinLanded?.Invoke();


        // Tell GameManager to record the score.

        OnThrowCompleted?.Invoke(
            true,
            distance,
            releaseQualityLabel
        );
    }


    // ====================================
    // RELEASE QUALITY
    // ====================================

    private float CalculateReleaseQuality(
        float distance
    )
    {
        if (distance > 3f)
            return 0.70f;

        if (distance > 1.5f)
            return 0.85f;

        if (distance > 0.5f)
            return 0.95f;

        return 1.00f;
    }


    // ====================================
    // RELEASE QUALITY LABEL
    // ====================================

    private string GetReleaseQualityLabel(
        float distance
    )
    {
        if (distance > 3f)
            return "EARLY";

        if (distance > 1.5f)
            return "SAFE";

        if (distance > 0.5f)
            return "GOOD";

        return "PERFECT";
    }


    // ====================================
    // STABILITY
    // ====================================

    private float CalculateStability(
        float speed
    )
    {
        if (speed <= 8f)
            return 1f;

        if (speed <= 9f)
            return 0.97f;

        return 0.93f;
    }


    // ====================================
    // RESET THROW
    // ====================================

    public void ResetThrow()
    {
        if (activeJavelin != null)
        {
            activeJavelin.OnJavelinLanded -=
                HandleJavelinLanding;

            Destroy(activeJavelin.gameObject);

            activeJavelin = null;
        }

        hasThrown = false;

        isPreparing = false;

        throwAngle = 35f;

        releaseQualityLabel = "";
    }


    // ====================================
    // PUBLIC PROPERTIES
    // ====================================

    public float CurrentThrowAngle
    {
        get { return throwAngle; }
    }

    public bool HasThrown
    {
        get { return hasThrown; }
    }
}