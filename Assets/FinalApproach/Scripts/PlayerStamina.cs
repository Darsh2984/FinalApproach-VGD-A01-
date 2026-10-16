
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float startingEnergy = 25f;
    public float drainRate = 3f;

    [Header("References")]
    public Slider energyBar;
    public CharacterController characterController;

    private float currentEnergy;

    void Start()
    {
        currentEnergy = Mathf.Clamp(startingEnergy, 0f, maxEnergy);

        if (energyBar != null)
        {
            energyBar.minValue = 0f;
            energyBar.maxValue = maxEnergy;
            energyBar.value = currentEnergy;
        }
    }

    void Update()
    {
        if (characterController == null)
            return;

        // Check horizontal movement only.
        Vector3 velocity = characterController.velocity;
        velocity.y = 0f;

        bool isMoving = velocity.magnitude > 0.1f;

        if (isMoving && currentEnergy > 0f)
        {
            currentEnergy -= drainRate * Time.deltaTime;
            currentEnergy = Mathf.Max(currentEnergy, 0f);
        }

        if (energyBar != null)
            energyBar.value = currentEnergy;
    }

    public void RestoreFullEnergy()
    {
        currentEnergy = maxEnergy;

        if (energyBar != null)
            energyBar.value = currentEnergy;
    }
}
