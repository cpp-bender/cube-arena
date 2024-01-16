using System.Collections;
using UnityEngine;

public class DecideState : AIBaseState
{
    public DecideState()
    {
        name = AIState.Decide;
    }

    public override IEnumerator Enter(AIController ai)
    {
        ai.isAttacking = false;

        if (ai.GetStackCount() >= 1)
        {
            ai.SwitchAnimation("Surf");
        }

        else if (ai.GetStackCount() == 0)
        {
            ai.SwitchAnimation("Idle");
        }

        yield return new WaitForSeconds(ai.aIData.DecideDuration);

        Decide();

        void Decide()
        {
            ai.SwitchState(ai.aIData.GetNextRandomState(ai));
        }
    }

    public override void Update(AIController ai)
    {

    }
}
