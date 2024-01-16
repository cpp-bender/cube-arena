using System.Collections;
using UnityEngine;

public class PlayerController : CharacterController, IGameState, IGateable
{
    [Header("DEPENDENCIES")]
    [SerializeField] InputData input;
    [SerializeField] PlayerData playerData;

    [Header("DEBUG")]
    public Platform platform;

    private AnimationStates animationState;
    private bool canMove = false;

    protected override void Start()
    {
        base.Start();

        platform = Platform.Instance;
    }

    private void Update()
    {
        if (canMove)
        {
            Vector3 moveDir = CalculateDirection();
            Movement(moveDir);

            Vector3 lookDirection = CalculateDirection();
            LookDirection(lookDirection);

            AnimationHandle();
        }
    }

    #region Movement
    private void Movement(Vector3 direction)
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + direction * playerData.Speed, Time.deltaTime);
    }

    private void LookDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
        }
    }

    private Vector3 CalculateDirection()
    {
        var forwardVec = (platform.transform.position - transform.position).normalized;

        var rightVector = Vector3.Cross(Vector3.up, forwardVec);

        return forwardVec * input.Vertical + rightVector * input.Horizontal;
    }
    #endregion

    #region GameState
    public void OnLevelEnd()
    {
        canMove = false;
    }

    public void OnLevelStart()
    {
        canMove = true;
    }
    #endregion

    #region Gate Issues
    public void IncreaseStackCount(int stackCount, Cube cubePrefab)
    {
        for (int i = 0; i < stackCount; i++)
        {
            if(!IsCharacterReachMaxStackCount())
            {
                var newCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);

                CollectCube(newCube, gameObject);
            }
        }
    }

    public void DecreaseStackCount(int stackCount)
    {
        // bölüm yapýlacak cube'ü bulup Drop etmek yeterli
    }

    public int StackCount()
    {
        return GetStackCount();
    }
    #endregion

    private void AnimationHandle()
    {
        if (input.Magnitude > 0)
        {
            if (GetStackCount() == 0)
            {
                if (animationState != AnimationStates.RUNNING)
                {
                    animationState = AnimationStates.RUNNING;
                    animator.SetTrigger("Run");
                }
            }
            else if (GetStackCount() >= 0)
            {
                if (animationState != AnimationStates.IDLE)
                {
                    animationState = AnimationStates.IDLE;
                    animator.SetTrigger("Idle");
                }
            }
        }
        else if (animationState != AnimationStates.IDLE)
        {
            if (animationState != AnimationStates.IDLE)
            {
                animationState = AnimationStates.IDLE;
                animator.SetTrigger("Idle");
            }
        }
    }

    protected override Material GetMaterial()
    {
        return playerData.Material;
    }

    public override ColorType GetColorType()
    {
        return playerData.ColorType;
    }

    public override void WinTheGame()
    {
        animator.SetTrigger("Dance");
        StartCoroutine(LevelComplete(2f));
        canMove = false;
    }

    public override void LoseTheGame()
    {
        animator.SetTrigger("Sad");
        StartCoroutine(LevelFail(2f));
        canMove = false;
    }

    private IEnumerator LevelComplete(float duration)
    {
        yield return new WaitForSeconds(duration);

        GameManager.instance.LevelComplete();
    }

    private IEnumerator LevelFail(float duration)
    {
        yield return new WaitForSeconds(duration);

        GameManager.instance.LevelFail();
    }
}

public enum AnimationStates
{
    IDLE,
    RUNNING,
    SURFING
}

