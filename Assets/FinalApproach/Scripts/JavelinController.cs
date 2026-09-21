using UnityEngine;
using System;

public class JavelinController : MonoBehaviour
{
    [Header("Javelin References")]

    [SerializeField]
    private Rigidbody rb;


    [Header("Flight State")]

    [SerializeField]
    private bool hasLanded = false;


    [Header("Throw Information")]

    [SerializeField]
    private float launchSpeed;

    [SerializeField]
    private float throwDistance;


    private float throwLineZ;

    private Collider landingFieldCollider;


    // Event triggered when the javelin lands.

    public event Action<float> OnJavelinLanded;


    public float ThrowDistance
    {
        get { return throwDistance; }
    }


    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }


    // ====================================
    // LAUNCH JAVELIN
    // ====================================

    public void Launch(
        Vector3 launchVelocity,
        float linePositionZ,
        Collider landingField)
    {
        throwLineZ = linePositionZ;

        landingFieldCollider = landingField;

        launchSpeed = launchVelocity.magnitude;

        hasLanded = false;

        throwDistance = 0f;

        rb.isKinematic = false;

        rb.useGravity = true;

        rb.linearVelocity = launchVelocity;

        rb.angularVelocity = Vector3.zero;

        Debug.Log(
            "Javelin launched at " +
            launchSpeed.ToString("F2") +
            " m/s"
        );
    }


    // ====================================
    // DETECT LANDING
    // ====================================

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded)
        {
            return;
        }

        if (collision.collider != landingFieldCollider)
        {
            return;
        }

        hasLanded = true;

        Vector3 landingPoint =
            collision.GetContact(0).point;

        throwDistance =
            landingPoint.z - throwLineZ;

        throwDistance =
            Mathf.Max(0f, throwDistance);


        Debug.Log(
            "JAVELIN LANDED! Distance: " +
            throwDistance.ToString("F2") +
            " metres"
        );


        rb.linearVelocity = Vector3.zero;

        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;


        // Notify the ThrowController.

        OnJavelinLanded?.Invoke(throwDistance);
    }
}