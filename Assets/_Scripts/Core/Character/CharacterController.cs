using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    [Header("Base Class Members")]
    [SerializeField] protected GameObject trailGO;
    [SerializeField] protected TrailRenderer trailRenderer;
    [SerializeField] protected Transform modelTransform;
    [SerializeField] protected DynamicTrigger dynamicTrigger;
    [SerializeField] protected Animator animator;

    private const int MAXCUBECOUNT = 23;
    private const float INVESTDURATION = 0.1f;
    protected const float YOFFSET = .7F;

    protected Vector3 lastStackPos = Vector3.zero;

    public List<Cube> cubes = new List<Cube>();
    public bool canInvest = true;

    private void RefreshDynamicTrigger() => dynamicTrigger.RefreshDynamicSettings(GetStackCount());

    protected virtual void Start()
    {
        CheckTrailActiveable();
        SetTrailRendererColor(GetMaterial().color);
    }

    #region Event Subscribe
    protected virtual void OnEnable()
    {
        EventManager.Instance.OnCharacterTriggerEnterCube += CollectCube;
        EventManager.Instance.OnObstacleTriggerEnterCube += DropCube;
        EventManager.Instance.OnCharacterTriggerStayInvestArea += StartInvestingCube;
        EventManager.Instance.OnCharacterTriggerExitInvestArea += StopInvestingCube;
        InvestManager.Instance.AddToInvesterList(this);
        CharacterCollisionManager.Instance.AddToList(this);
    }

    protected virtual void OnDisable()
    {
        EventManager.Instance.OnCharacterTriggerEnterCube -= CollectCube;
        EventManager.Instance.OnObstacleTriggerEnterCube -= DropCube;
        EventManager.Instance.OnCharacterTriggerStayInvestArea -= StartInvestingCube;
        EventManager.Instance.OnCharacterTriggerExitInvestArea -= StopInvestingCube;
        InvestManager.Instance.RemoveFromInvesterList(this);
        CharacterCollisionManager.Instance.RemoveFromList(this);
    }
    #endregion

    public void SwitchAnimation(string triggerName)
    {
        var currentAnimState = animator.GetCurrentAnimatorStateInfo(0);

        if (!currentAnimState.IsName(triggerName))
        {
            animator.SetTrigger(triggerName);
        }
    }

    public bool HasAtLeastOneStack()
    {
        return GetStackCount() >= 1;
    }

    public float GetModelLocalPosY()
    {
        return modelTransform.localPosition.y;
    }

    #region Cube Collect, Drop And Invest
    public virtual void CollectCube(Cube cube, GameObject go)
    {
        if (cube.CanCollectable() && go == gameObject && GetStackCount() < MAXCUBECOUNT)
        {
            cube.Collect(this, CalculateStackPos(), GetMaterial());
            cubes.Insert(0, cube);
            ModelLocalJump();
            RefreshDynamicTrigger();
            CheckTrailActiveable();
        }
    }

    public virtual void DropCube(Cube cube, CharacterController characterController)
    {
        if (cube.CanDropable() && characterController == this)
        {
            if (cube != cubes[cubes.Count - 1])
            {
                var cubeIndex = cubes.IndexOf(cube);
                var underCube = cubes[cubeIndex + 1];

                DropCube(underCube, this);
            }

            CalculateStackPos();

            cube.Drop();
            cubes.Remove(cube);

            RefreshDynamicTrigger();
            CheckTrailActiveable();
        }
    }

    public virtual void DropAllCube()
    {
        if (GetStackCount() > 0)
        {
            DropCube(cubes[0], this);
        }
    }

    public virtual void StartInvestingCube(GameObject go)
    {
        if (go == gameObject && canInvest)
        {
            InvestCube();
        }
    }

    public virtual void StopInvestingCube(GameObject go)
    {
        if (go == gameObject)
        {
            StopAllCoroutines();
            StartCoroutine(ResetInvestSetting(0f));
        }
    }

    public virtual void InvestCube()
    {
        if (GetStackCount() > 0)
        {
            canInvest = false;
            StartCoroutine(ResetInvestSetting(INVESTDURATION));

            Cube lastCube = cubes[GetStackCount() - 1];
            cubes.Remove(lastCube);

            InvestManager.Instance.InvestCube(GetColorType(), lastCube);

            RefreshDynamicTrigger();

            CheckTrailActiveable();

            LevelManager.Instance.currentCubeCount--;

            if (LevelManager.Instance.currentCubeCount < 40)
            {
                LevelManager.Instance.SpawnOneCube();
            }
        }
    }

    private IEnumerator ResetInvestSetting(float duration)
    {
        yield return new WaitForSeconds(duration);

        canInvest = true;
    }

    protected virtual Vector3 CalculateStackPos()
    {
        if (cubes.Count > 0)
        {
            lastStackPos = modelTransform.localPosition;
        }
        else
        {
            lastStackPos = Vector3.zero;
        }

        return lastStackPos;
    }

    protected virtual void ModelLocalJump()
    {
        modelTransform.localPosition += Vector3.up * YOFFSET;
    }

    public virtual int GetStackCount()
    {
        return cubes.Count;
    }

    public bool IsCharacterReachMaxStackCount()
    {
        if (GetStackCount() < MAXCUBECOUNT)
            return false;
        else
            return true;
    }
    #endregion

    #region Trail
    private void CheckTrailActiveable()
    {
        if (GetStackCount() > 0)
            SwitchTrail(true);
        else
            SwitchTrail(false);
    }

    private void SwitchTrail(bool isActive)
    {
        trailGO.SetActive(isActive);

        if (!isActive)
            trailRenderer.Clear();
    }

    private void SetTrailRendererColor(Color color)
    {
        trailRenderer.startColor = color;

        var endColor = color;
        endColor.a = 0.3f;

        trailRenderer.endColor = endColor;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.CHARACTER))
        {
            CharacterCollisionManager.Instance.CheckCollision(this, collision.gameObject);
        }
    }

    protected abstract Material GetMaterial();

    public abstract ColorType GetColorType();

    public abstract void WinTheGame();

    public abstract void LoseTheGame();
}
