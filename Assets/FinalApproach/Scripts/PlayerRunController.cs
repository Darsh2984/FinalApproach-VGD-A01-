using UnityEngine;

public class PlayerRunController : MonoBehaviour
{
    // ==================================================
    // 1. RUNNING SPEED SETTINGS
    // ==================================================

    [Header("Running Speed")]

    [SerializeField]
    private float baseSpeed = 3.0f;

    [SerializeField]
    private float maxSpeed = 10.0f;

    [SerializeField]
    private float currentSpeed;


    // ==================================================
    // 2. ACCELERATION SETTINGS
    // ==================================================

    [Header("Acceleration")]

    [SerializeField]
    private float goodTapAcceleration = 0.45f;

    [SerializeField]
    private float badTapAcceleration = 0.10f;


    // ==================================================
    // 3. RHYTHM SETTINGS
    // ==================================================

    [Header("Sprint Rhythm")]

    [SerializeField]
    private float minimumGoodTapInterval = 0.18f;

    [SerializeField]
    private float maximumGoodTapInterval = 0.32f;


    // ==================================================
    // 4. SPEED DECAY SETTINGS
    // ==================================================

    [Header("Speed Decay")]

    [SerializeField]
    private float decayDelay = 0.50f;

    [SerializeField]
    private float decayRate = 2.0f;


    // ==================================================
    // 5. PLAYER STATE
    // ==================================================

    [Header("Player State")]

    [SerializeField]
    private bool canRun = true;


    // ==================================================
    // 6. INTERNAL VARIABLES
    // ==================================================

    private float lastTapTime;

    private bool hasTapped;


    // ==================================================
    // 7. PUBLIC PROPERTIES
    // ==================================================

    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    public bool CanRun
    {
        get { return canRun; }
    }


    // ==================================================
    // 8. INITIALIZATION
    // ==================================================

    private void Start()
    {
        currentSpeed = baseSpeed;

        lastTapTime = 0f;

        hasTapped = false;
    }


    // ==================================================
    // 9. MAIN GAME LOOP
    // ==================================================

    private void Update()
    {
        if (!canRun)
        {
            return;
        }

        HandleSprintInput();

        HandleSpeedDecay();

        MovePlayer();
    }


    // ==================================================
    // 10. HANDLE SPACE TAPPING
    // ==================================================

    private void HandleSprintInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float currentTime = Time.time;

            // First tap always receives full acceleration.

            if (!hasTapped)
            {
                currentSpeed += goodTapAcceleration;

                hasTapped = true;
            }
            else
            {
                float tapInterval =
                    currentTime - lastTapTime;

                // Player taps at the correct rhythm.

                if (tapInterval >= minimumGoodTapInterval &&
                    tapInterval <= maximumGoodTapInterval)
                {
                    currentSpeed += goodTapAcceleration;
                }

                // Player taps too quickly.

                else if (tapInterval < minimumGoodTapInterval)
                {
                    currentSpeed += badTapAcceleration;
                }

                // Player taps too slowly.

                else
                {
                    currentSpeed += badTapAcceleration;
                }
            }

            // Never exceed maximum running speed.

            currentSpeed = Mathf.Clamp(
                currentSpeed,
                baseSpeed,
                maxSpeed
            );

            lastTapTime = currentTime;
        }
    }


    // ==================================================
    // 11. HANDLE SPEED DECAY
    // ==================================================

    private void HandleSpeedDecay()
    {
        if (!hasTapped)
        {
            return;
        }

        float timeSinceLastTap =
            Time.time - lastTapTime;

        // Begin slowing down when the player
        // has stopped tapping for too long.

        if (timeSinceLastTap > decayDelay)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                baseSpeed,
                decayRate * Time.deltaTime
            );
        }
    }


    // ==================================================
    // 12. MOVE THE PLAYER
    // ==================================================

    private void MovePlayer()
    {
        Vector3 movement =
            Vector3.forward *
            currentSpeed *
            Time.deltaTime;

        transform.Translate(
            movement,
            Space.World
        );
    }


    // ==================================================
    // 13. STOP PLAYER MOVEMENT
    // ==================================================

    public void StopRunning()
    {
        canRun = false;
    }


    // ==================================================
    // 14. RESUME PLAYER MOVEMENT
    // ==================================================

    public void ResumeRunning()
    {
        canRun = true;
    }


    // ==================================================
    // 15. RESET PLAYER RUNNING STATE
    // ==================================================

    public void ResetRunning()
    {
        currentSpeed = baseSpeed;

        lastTapTime = 0f;

        hasTapped = false;

        canRun = true;
    }
}