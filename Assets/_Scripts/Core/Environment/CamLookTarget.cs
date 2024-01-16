using UnityEngine;

public class CamLookTarget : MonoBehaviour
{
    private HyperCameraController cam;

    private void Start()
    {
        cam = Camera.main.GetComponent<HyperCameraController>();
    }

    private void LateUpdate()
    {
        transform.localRotation = cam.transform.rotation;
    }
}
