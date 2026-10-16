
using UnityEngine;

public class ProximityLight : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Light targetLight;

    [Header("Settings")]
    public float activationDistance = 5f;

    void Start()
    {
        if (targetLight != null)
            targetLight.enabled = false;
    }

    void Update()
    {
        if (player == null || targetLight == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        targetLight.enabled = distance <= activationDistance;
    }
}
