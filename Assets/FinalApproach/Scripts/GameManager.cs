using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ====================================
    // REFERENCES
    // ====================================

    [Header("Player References")]

    [SerializeField]
    private Transform player;

    [SerializeField]
    private PlayerRunController runController;

    [SerializeField]
    private ThrowController throwController;

    [Header("Camera Reference")]

    [SerializeField]
    private CameraDirector cameraDirector;
    
    [Header("How to Play")]
    [SerializeField] private GameObject howToPlayPanel;

    private bool tutorialOpen = false;



    // ====================================
    // UI REFERENCES
    // ====================================

    [Header("UI References")]

    [SerializeField]
    private TMP_Text attemptText;

    [SerializeField]
    private TMP_Text resultText;


    // ====================================
    // COMPETITION SETTINGS
    // ====================================

    [Header("Competition Settings")]

    [SerializeField]
    private int maximumAttempts = 3;


    // ====================================
    // SCORE VARIABLES
    // ====================================

    [Header("Scores")]

    [SerializeField]
    private int currentAttempt = 1;

    [SerializeField]
    private float roundBest = 0f;

    [SerializeField]
    private float personalBest = 0f;

    [SerializeField]
    private float lastDistance = 0f;


    // ====================================
    // INTERNAL STATE
    // ====================================

    private Vector3 startingPosition;

    private Quaternion startingRotation;

    private bool isAttemptComplete = false;

    private bool isRoundComplete = false;


    // ====================================
    // INITIALIZATION
    // ====================================

    private void Start()
    {
        startingPosition =
            player.position;

        startingRotation =
            player.rotation;


        currentAttempt = 1;

        roundBest = 0f;

        personalBest = 0f;

        lastDistance = 0f;


        // Listen for completed throws.

        throwController.OnThrowCompleted +=
            HandleThrowCompleted;


        UpdateAttemptUI();

        resultText.text = "";

        ShowTutorial();

    }


    // ====================================
    // UPDATE
    // ====================================

    private void Update()
    {
        // H opens or closes instructions during the run-up.
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (tutorialOpen)
            {
                StartGameFromTutorial();
            }
            else
            {
                ShowTutorial();
            }
        }

        // Don't process Enter while instructions are open.
        if (tutorialOpen)
        {
            return;
        }

        if (!isAttemptComplete)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isRoundComplete)
            {
                StartNewRound();
            }
            else
            {
                StartNextAttempt();
            }
        }
    }


    // ====================================
    // CLEAR OPENING MESSAGE
    // ====================================

    private void ClearOpeningMessage()
    {
        if (!isAttemptComplete)
        {
            resultText.text = "";
        }
    }


    // ====================================
    // THROW COMPLETED
    // ====================================

    private void HandleThrowCompleted(
        bool isValid,
        float distance,
        string releaseQuality
    )
    {
        isAttemptComplete = true;


        if (!isValid)
        {
            lastDistance = 0f;

            resultText.text =
                "FOUL!\n\n" +
                "The athlete crossed the line.\n\n";
        }
        else
        {
            lastDistance = distance;


            if (distance > roundBest)
            {
                roundBest = distance;
            }


            if (distance > personalBest)
            {
                personalBest = distance;
            }


            resultText.text =
                "THROW RESULT\n\n" +

                "Distance: " +
                distance.ToString("F2") +
                " m\n" +

                "Release: " +
                releaseQuality +
                "\n\n";
        }


        // Check whether all attempts are finished.

        if (currentAttempt >= maximumAttempts)
        {
            CompleteRound();
        }
        else
        {
            resultText.text +=
                "Round Best: " +
                roundBest.ToString("F2") +
                " m\n\n" +

                "Personal Best: " +
                personalBest.ToString("F2") +
                " m\n\n" +

                "PRESS ENTER FOR NEXT ATTEMPT";
        }
    }


    // ====================================
    // COMPLETE ROUND
    // ====================================

    private void CompleteRound()
    {
        isRoundComplete = true;


        resultText.text +=
            "COMPETITION COMPLETE!\n\n" +

            "Round Best: " +
            roundBest.ToString("F2") +
            " m\n\n" +

            "Personal Best: " +
            personalBest.ToString("F2") +
            " m\n\n" +

            "PRESS ENTER FOR NEW ROUND";
    }


    // ====================================
    // NEXT ATTEMPT
    // ====================================

    private void StartNextAttempt()
    {
        currentAttempt++;

        ResetPlayerAndThrow();

        UpdateAttemptUI();
    }


    // ====================================
    // NEW ROUND
    // ====================================

    private void StartNewRound()
    {
        currentAttempt = 1;

        roundBest = 0f;

        lastDistance = 0f;

        isRoundComplete = false;


        // Personal best is intentionally kept.

        ResetPlayerAndThrow();

        UpdateAttemptUI();
    }


    // ====================================
    // RESET PLAYER AND THROW
    // ====================================

    private void ResetPlayerAndThrow()
    {
        // Reset the player to starting location.

        player.position =
            startingPosition;

        player.rotation =
            startingRotation;


        // Reset throw system.

        throwController.ResetThrow();


        // Reset running speed and input state.

        runController.ResetRunning();

        // Return camera to athlete.

        cameraDirector.ResetToPlayer();


        isAttemptComplete = false;

        resultText.text = "";
    }


    // ====================================
    // UPDATE ATTEMPT UI
    // ====================================

    private void UpdateAttemptUI()
    {
        attemptText.text =
            "ATTEMPT: " +
            currentAttempt +
            " / " +
            maximumAttempts;
    }

    public void ShowTutorial()
    {
        // Don't interrupt a throw or a completed result.
        if (isAttemptComplete || throwController.HasThrown)
        {
            return;
        }

        tutorialOpen = true;

        howToPlayPanel.SetActive(true);

        runController.StopRunning();

        throwController.enabled = false;
    }


    public void StartGameFromTutorial()
    {
        tutorialOpen = false;

        howToPlayPanel.SetActive(false);

        throwController.enabled = true;

        runController.ResumeRunning();
    }

    // ====================================
    // CLEANUP
    // ====================================

    private void OnDestroy()
    {
        if (throwController != null)
        {
            throwController.OnThrowCompleted -=
                HandleThrowCompleted;
        }
    }
}