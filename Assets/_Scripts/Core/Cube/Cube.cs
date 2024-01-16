using System.Collections;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class Cube : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider collider;

    [Header("Cube Data"), Space(5f)]
    [SerializeField] float speed = 20f;
    [SerializeField] Material neutralMaterial;
    [SerializeField] PhysicMaterial neutralPhysicMaterial;
    [SerializeField] PhysicMaterial collectedPhysicMaterial;

    [Header("DEBUG")]
    public AIController targetAI;
    public bool isCollected = false;

    private CharacterController character;
    private bool isInvested = false;
    private Tween spawnTween;

    public Tween DoSpawnAnimation()
    {
        var initSeq = DOTween.Sequence();

        float seqDuration = .5f;

        TweenParams settings = new TweenParams().SetEase(Ease.OutQuad).SetRelative(true);

        Tween moveTween = transform.DOMoveY(2f, seqDuration).SetAs(settings);

        Tween rotateTween = transform.DORotate(new Vector3(0f, 180f, 0f), seqDuration, RotateMode.LocalAxisAdd).SetAs(settings);

        initSeq.Append(moveTween).Join(rotateTween).SetLoops(2, LoopType.Yoyo);

        initSeq
            .OnStart(delegate
            {
                rb.isKinematic = true;
            })
            .OnComplete(delegate
            {
                rb.isKinematic = false;
            })
            .OnKill(() => rb.isKinematic = false);

        spawnTween = initSeq;

        return initSeq.Play();
    }

    public void Collect(CharacterController character, Vector3 stackPos, Material material)
    {
        if (!isCollected || !isInvested)
        {
            tag = "Untagged";
            spawnTween.Kill(false);
            isCollected = true;
            RbCollectSetUp();
            SetTransformParent(character.transform);
            GoToLocalStackPosition(stackPos);
            SetCubeMaterial(material);
            LocalRotationReset();
            this.character = character;
        }
    }

    public void Drop()
    {
        if (isCollected || !isInvested)
        {
            isCollected = false;
            RbDropSetUp();
            SetTransformParent(null);
            RandomThrow();
            SetCubeMaterial(neutralMaterial);
            character = null;
            targetAI = null;
            
            isCollected = true;
            StartCoroutine(ResetCollectSetting(0.25f));
        }
    }

    public void Invest(Transform receiverCharacterTransform, Vector3 localInvestPos, Material material)
    {
        isInvested = true;
        RbInvestSetup();
        SetTransformParent(receiverCharacterTransform);
        GoToLocalInvestPosition(localInvestPos);
        LocalRotationReset();
        SetCubeMaterial(material);
        character = null;
    }

    public void Invest(Transform receiverCharacterTransform, Vector3 localInvestPos)
    {
        if ((isCollected && !isInvested))
        {
            isInvested = true;
            RbInvestSetup();
            SetTransformParent(receiverCharacterTransform);
            GoToLocalInvestPosition(localInvestPos);
            LocalRotationReset();
            character = null;
        }
    }

    public void Free()
    {
        if(isInvested)
        {
            SetTransformParent(null);
            RbFreeSettings();
            RandomFreeThrow();
        }
    }

    public float FindArrivalTime(Vector3 arrivalPos)
    {
        return Vector3.Distance(transform.position, arrivalPos) / speed;
    }

    private IEnumerator ResetCollectSetting(float duration)
    {
        yield return new WaitForSeconds(duration);

        isCollected = false;
        tag = "Cube";
    }

    private void GoToLocalStackPosition(Vector3 localPos)
    {
        transform.localPosition = localPos;
    }

    private void GoToLocalInvestPosition(Vector3 localPos)
    {
        transform.localPosition = localPos;
    }

    private void RandomThrow()
    {
        rb.AddForce(-transform.forward * Random.Range(300, 600), ForceMode.Acceleration);
        rb.AddForce(transform.right * Random.Range(300, 600), ForceMode.Acceleration);
        rb.AddForce(-transform.up * Random.Range(600, 1200), ForceMode.Acceleration);
    }

    private void RandomFreeThrow()
    {
        rb.AddForce(Vector3.right * Random.Range(-100, 100), ForceMode.Acceleration);
        rb.AddForce(Vector3.forward * Random.Range(-100, 100), ForceMode.Acceleration);
    }

    private void SetTransformParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    private void SetCubeMaterial(Material material)
    {
        if (material != null)
            meshRenderer.material = material;
    }

    private void LocalRotationReset()
    {
        transform.localRotation = Quaternion.identity;
    }

    #region Rb SetUp
    private void RbCollectSetUp()
    {
        rb.velocity = Vector3.zero;
        SwitchRbConstraints();
        SetPhysicMaterial(collectedPhysicMaterial);
    }

    private void RbDropSetUp()
    {
        SwitchRbConstraints();
        SetPhysicMaterial(neutralPhysicMaterial);
    }

    private void RbInvestSetup()
    {
        rb.isKinematic = true;
    }

    private void RbFreeSettings()
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    private void SwitchRbConstraints()
    {
        switch (isCollected)
        {
            case true:
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX
                    | RigidbodyConstraints.FreezeRotation;
                break;

            case false:
                rb.constraints = RigidbodyConstraints.None;
                break;
        }
    }

    private void SetPhysicMaterial(PhysicMaterial physicMaterial)
    {
        collider.material = physicMaterial;
    }
    #endregion

    public CharacterController GetCollecter()
    {
        return character;
    }

    #region Interactable
    public bool CanCollectable()
    {
        return !isCollected;
    }

    public bool CanDropable()
    {
        return isCollected;
    }
    #endregion
}
