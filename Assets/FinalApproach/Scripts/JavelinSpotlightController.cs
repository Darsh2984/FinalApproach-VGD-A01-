
using UnityEngine;

public class JavelinSpotlightController : MonoBehaviour
{
    [Header("References")]
    public ThrowController throwController;
    public Light spotlight;

    private void Start()
    {
        if (spotlight != null)
            spotlight.enabled = false;
    }

    private void OnEnable()
    {
        if (throwController == null)
            return;

        throwController.OnJavelinLaunched += OnJavelinLaunched;
        throwController.OnJavelinLanded += OnJavelinLanded;
    }

    private void OnDisable()
    {
        if (throwController == null)
            return;

        throwController.OnJavelinLaunched -= OnJavelinLaunched;
        throwController.OnJavelinLanded -= OnJavelinLanded;
    }

    private void OnJavelinLaunched(Transform javelin)
    {
        if (spotlight != null)
            spotlight.enabled = true;
    }

    private void OnJavelinLanded()
    {
        if (spotlight != null)
            spotlight.enabled = false;
    }
}
