using UnityEngine;
using TMPro;

public class GameHUDController : MonoBehaviour
{
    // ========================================
    // PLAYER REFERENCES
    // ========================================

    [Header("Player References")]

    [SerializeField]
    private Transform player;

    [SerializeField]
    private PlayerRunController runController;

    [SerializeField]
    private ThrowController throwController;


    // ========================================
    // UI REFERENCES
    // ========================================

    [Header("UI References")]

    [SerializeField]
    private TMP_Text speedText;

    [SerializeField]
    private TMP_Text throwAngleText;

    [SerializeField]
    private TMP_Text distanceToLineText;


    // ========================================
    // THROW LINE SETTINGS
    // ========================================

    [Header("Throw Line")]

    [SerializeField]
    private float throwLineZ = 0f;


    // ========================================
    // UPDATE HUD
    // ========================================

    private void Update()
    {
        UpdateSpeed();

        UpdateThrowAngle();

        UpdateDistanceToLine();
    }


    // ========================================
    // DISPLAY RUNNING SPEED
    // ========================================

    private void UpdateSpeed()
    {
        float speed =
            runController.CurrentSpeed;

        speedText.text =
            "SPEED: " +
            speed.ToString("F2") +
            " m/s";
    }


    // ========================================
    // DISPLAY THROW ANGLE
    // ========================================

    private void UpdateThrowAngle()
    {
        float angle =
            throwController.CurrentThrowAngle;

        throwAngleText.text =
            "THROW ANGLE: " +
            angle.ToString("F1") +
            "°";
    }


    // ========================================
    // DISPLAY DISTANCE TO THROW LINE
    // ========================================

    private void UpdateDistanceToLine()
    {
        float distance =
            throwLineZ - player.position.z;

        // Player has already crossed the line.

        if (distance < 0f)
        {
            distanceToLineText.text =
                "DISTANCE TO LINE: FOUL ZONE";

            distanceToLineText.color =
                Color.red;
        }
        else
        {
            distanceToLineText.text =
                "DISTANCE TO LINE: " +
                distance.ToString("F2") +
                " m";

            distanceToLineText.color =
                Color.white;
        }
    }
}