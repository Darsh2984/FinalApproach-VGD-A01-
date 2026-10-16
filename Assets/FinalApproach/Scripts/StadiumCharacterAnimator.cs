
using UnityEngine;

public class StadiumCharacterAnimator : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;

    [Header("Animation Settings")]
    public float animationSpeedMultiplier = 1f;
    public float minimumMovementSpeed = 0.05f;

    private Vector3 previousPosition;

    void Start()
    {
        if (player == null)
            player = transform.parent;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (player != null)
            previousPosition = player.position;
    }

    void LateUpdate()
    {
        if (player == null || animator == null)
            return;

        Vector3 movement = player.position - previousPosition;
        movement.y = 0f;

        float speed = movement.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);

        float animationSpeed = speed * animationSpeedMultiplier;

        if (speed < minimumMovementSpeed)
            animationSpeed = 0f;

        animator.SetFloat("Speed", animationSpeed, 0.1f, Time.deltaTime);
        animator.SetFloat("MotionSpeed", 1f);

        previousPosition = player.position;
    }
}
