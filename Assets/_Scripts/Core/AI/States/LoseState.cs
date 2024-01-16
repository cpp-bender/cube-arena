using System.Collections;

public class LoseState : AIBaseState
{
    public LoseState()
    {
        name = AIState.Lose;
    }

    public override IEnumerator Enter(AIController ai)
    {
        ai.SwitchAnimation("Lose");
     
        yield return null;
    }

    public override void Update(AIController ai)
    {

    }
}
