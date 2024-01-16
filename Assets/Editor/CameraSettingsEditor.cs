using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraSettings))]
public class CameraSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.enabled = Application.isEditor;

        if (GUILayout.Button("Sync Position"))
        {
            var player = FindObjectOfType<PlayerController>();

            var cam = FindObjectOfType<HyperCameraController>();

            if (player == null)
            {
                Debug.LogWarning("Put player in the scene");
                return;
            }

            if (cam == null)
            {
                Debug.LogWarning("Put hyper cam in the scene");
                return;
            }

            cam.SetTransform();
        }
    }
}
