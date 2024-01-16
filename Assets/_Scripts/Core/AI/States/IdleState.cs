using System.Collections;

public class IdleState : AIBaseState
{
    public IdleState()
    {
        name = AIState.Idle;
    }

    public override IEnumerator Enter(AIController ai)
    {
        ai.SwitchAnimation("Idle");

        yield return null;
    }

    public override void Update(AIController ai)
    {

    }
}