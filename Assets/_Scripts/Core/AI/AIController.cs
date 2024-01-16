using UnityEngine;

public class AIController : CharacterController, IGameState
{
    [Header("COMPONENTS")]
    public SkinnedMeshRenderer renderer;

    [Header("DEPENDENCIES")]
    public AIData aIData;

    [Header("DEBUG")]
    public AIState stateName;
    public bool isAttacking;

    private AIBaseState currentState;

    private void Awake()
    {
        renderer.material = aIData.Material;
    }

    protected override void Start()
    {
        base.Start();
        SwitchState(new IdleState());

    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void SwitchState(AIBaseState state)
    {
        currentState = state;

        stateName = state.name;

        StartCoroutine(currentState.Enter(this));
    }

    public override int GetStackCount()
    {
        return base.GetStackCount();
    }

    public override ColorType GetColorType()
    {
        return aIData.ColorType;
    }

    protected override Material GetMaterial()
    {
        return aIData.Material;
    }

    public override void StartInvestingCube(GameObject go)
    {
        if (!isAttacking)
        {
            base.StartInvestingCube(go);
        }
    }

    public override void DropAllCube()
    {
        SwitchAnimation("Run");

        base.DropAllCube();
    }

    #region GAME STATE CALLBACKS
    public void OnLevelStart()
    {
        SwitchState(new SearchState());
    }
    public void OnLevelEnd()
    {
        SwitchState(new IdleState());
    }

    public override void WinTheGame()
    {
        SwitchState(new WinState());
    }

    public override void LoseTheGame()
    {
        SwitchState(new LoseState());
    }
    #endregion
}
