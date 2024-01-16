using UnityEngine;

[CreateAssetMenu(menuName = "Cube Arena/Camera Settings", fileName = "Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [SerializeField] Vector3 offset;
    [SerializeField, Range(0f, 1f)] float smoothTime;

    private Vector3 velocity = Vector3.zero;

    public Vector3 Offset { get => offset; }
    public float SmoothTime { get => smoothTime; }
    public Vector3 Velocity { get => velocity; }
}
