using System.Collections;

public class WinState : AIBaseState
{
    public WinState()
    {
        name = AIState.Win;
    }

    public override IEnumerator Enter(AIController ai)
    {
        ai.SwitchAnimation("Win");

        yield return null;
    }

    public override void Update(AIController ai)
    {

    }
}
