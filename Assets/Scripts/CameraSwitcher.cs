using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Target Camera")]
    public Camera targetCamera;

    [Header("Disable Camera")]
    public Camera cameraToDisable;

    [Header("Collider")]
    public bool useTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger) return;
        SwitchCamera();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (useTrigger) return;
        SwitchCamera();
    }

    private void SwitchCamera()
    {
        if (targetCamera == null)
        {
            return;
        }

        if (cameraToDisable != null)
        {
            cameraToDisable.gameObject.SetActive(false);
        }
        else
        {
            Camera[] allCameras = Camera.allCameras;
            foreach (Camera cam in allCameras)
            {
                cam.gameObject.SetActive(false);
            }
        }

        targetCamera.gameObject.SetActive(true);
    }
}