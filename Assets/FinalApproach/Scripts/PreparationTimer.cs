
using UnityEngine;
using TMPro;
using System;

public class PreparationTimer : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;

    [Header("Settings")]
    public float preparationTime = 10f;

    public event Action OnTimeExpired;

    private float timeRemaining;
    private bool isRunning;

    public bool HasTimeRemaining => timeRemaining > 0f;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!isRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;

            UpdateDisplay();
            OnTimeExpired?.Invoke();
            return;
        }

        UpdateDisplay();
    }

    public void ResetTimer()
    {
        timeRemaining = preparationTime;
        isRunning = true;
        UpdateDisplay();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateDisplay()
    {
        if (timerText == null)
            return;

        timerText.text =
            "TIME: " + Mathf.CeilToInt(timeRemaining);

        timerText.color = timeRemaining <= 3f
            ? Color.red
            : Color.white;
    }
}
