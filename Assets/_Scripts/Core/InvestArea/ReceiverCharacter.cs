using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ReceiverCharacter : MonoBehaviour
{
    [Header("DEPENDENCIES")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] TextMeshProUGUI placementText;
    [SerializeField] Transform modelTransform;
    [SerializeField] SkinnedMeshRenderer skinnedRenderer;
    [SerializeField] CharacterData data;
    [SerializeField] Cube investCubePrefab;
    [SerializeField] int startCubeCount;

    public ColorType ColorType { get => data.ColorType; }

    private const float YOFFSET = .65f;

    private bool canReceive = true;
    private int placement;
    private int investedCubeCount;
    private int realInvestedCubeCount;
    private Vector3 lastStackPos;
    private List<Cube> stack = new List<Cube>();

    private void Awake()
    {
        skinnedRenderer.material = data.Material;
    }

    private void Start()
    {
        for (int i = 0; i < startCubeCount; i++)
        {
            Cube tempCube = Instantiate(investCubePrefab, transform);
            tempCube.isCollected = true;
            tempCube.Invest(transform, ACubeInvested(0f, tempCube), data.Material);
        }

        investedCubeCount -= startCubeCount;
    }

    public Vector3 CalculateLocalStackPos()
    {
        lastStackPos = new Vector3(0f, realInvestedCubeCount * YOFFSET, 0f);

        return lastStackPos;
    }

    public Vector3 ACubeInvested(float arrivalTime, Cube cube)
    {
        if (canReceive)
        {
            investedCubeCount++;
            realInvestedCubeCount++;

            stack.Add(cube);

            StartCoroutine(ModelLocalJump(arrivalTime));
            return CalculateLocalStackPos();
        }
        else
        {
            return Vector3.one * -5;
        }

    }

    public void SetPlacement(int placement)
    {
        this.placement = placement;
        placementText.SetText(placement.ToString());

        HandleAnimatoin();
    }

    public int GetInvestedCubeCount()
    {
        return investedCubeCount;
    }

    public void WinTheGame()
    {
        canReceive = false;
        placementText.gameObject.SetActive(false);
        AnimatoinWin();
    }

    public void LoseTheGame()
    {
        canReceive = false;
        placementText.gameObject.SetActive(false);

        RbLoseSettings();
        AnimatoinLose();
        FreeAllCubes();
    }

    private void FreeAllCubes()
    {
        for (int i = 0; i < stack.Count; i++)
        {
            stack[i].Free();
        }
    }

    private void RbLoseSettings()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        //rb.constraints = RigidbodyConstraints.None;
    }

    private IEnumerator ModelLocalJump(float duration)
    {
        yield return new WaitForSeconds(duration);

        modelTransform.localPosition += Vector3.up * YOFFSET;
    }

    private void HandleAnimatoin()
    {
        if (placement == 1)
        {
            animator.SetTrigger("Dance");
        }
        else if (placement == 2)
        {
            animator.SetTrigger("Sad");
        }
        else if (placement > 2)
        {
            animator.SetTrigger("LosingBalance");
        }
    }

    private void AnimatoinLose()
    {
        animator.SetTrigger("Falling");
    }

    private void AnimatoinWin()
    {
        animator.SetTrigger("Dance");
    }
}
