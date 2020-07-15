using System.Collections;
using UnityEngine;

public class CalibrateTrackableEventHandler : DefaultTrackableEventHandler
{
    private int filterCounter = 0;
    protected override void OnTrackingFound()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;

            StartCoroutine(CalibSpace(rendererComponents));
        }

        IEnumerator CalibSpace(Renderer[] rendererComponents)
        {
            // wait for 20 frames, if then marker still tracked use it to calibrate
            while (filterCounter < 20)
            {
                filterCounter += 1;
                yield return null;
            }
            if (rendererComponents[0].enabled == true)
            {
                // Calibration done;
                filterCounter = 0;
                GetComponent<CalibrationVuforia>().CalibrationDone();
            }
            yield return null;
        }
    }


}
