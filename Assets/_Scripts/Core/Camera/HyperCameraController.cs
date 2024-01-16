using UnityEngine;

public class HyperCameraController : SingletonMonoBehaviour<HyperCameraController>
{
    [Header("DEPENDENCIES")]
    public CameraSettings settings;
    public CamLookTarget lookTarget;

    [Header("DEBUG")]
    public PlayerController player;
    public float angle;

    private Vector3 velocity;
    private Vector3 lookDir;

    private Vector3 startOffset;
    private Vector3 relativePosOfPlayerToLookTarget;
    private Vector3 relativePosOfCamToPlayer;
    private float heightOfCam;
    private float angleOfCam;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        startOffset = transform.position - player.transform.position;
        heightOfCam = transform.position.y;
        angleOfCam = transform.rotation.eulerAngles.x;

    }

    private void LateUpdate()
    {
        angle = Vector3.Angle(transform.position, lookTarget.transform.position);

        Look();

        Follow();
    }

    private void Look()
    {
        // lookDir = (lookTarget.transform.position - player.transform.position).normalized;
        
        lookDir = (lookTarget.transform.position - transform.position).normalized;
        Vector3 lookRotation = Quaternion.LookRotation(lookDir).eulerAngles;
        lookRotation.x = angleOfCam;

        transform.rotation = Quaternion.Euler(lookRotation);
    }

    private void Follow()
    {
        // Vector3 nextPos = player.transform.position + transform.forward * settings.Offset.z + Vector3.up * settings.Offset.y;

        relativePosOfPlayerToLookTarget = player.transform.position - lookTarget.transform.position;
        relativePosOfCamToPlayer = relativePosOfPlayerToLookTarget.normalized *
                                   Mathf.Sqrt(startOffset.x * startOffset.x + startOffset.z * startOffset.z);
        
        Vector3 nextPos = lookTarget.transform.position + relativePosOfCamToPlayer + relativePosOfPlayerToLookTarget;
        nextPos.y = heightOfCam;

        transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, settings.SmoothTime);
    }

    public void SetTransform()
    {
        Vector3 nextPos = player.transform.position + transform.forward * settings.Offset.z + Vector3.up * settings.Offset.y;

        transform.position = nextPos;

        lookDir = (lookTarget.transform.position - player.transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
