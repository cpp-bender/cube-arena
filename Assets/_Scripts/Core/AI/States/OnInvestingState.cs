using System.Collections;
using UnityEngine;

public class OnInvestingState : AIBaseState
{
    public override IEnumerator Enter(AIController ai)
    {
        ai.SwitchAnimation("Idle");

        yield return new WaitForEndOfFrame();  

        canUpdate = true;

        yield return null;
    }

    public override void Update(AIController ai)
    {
        if (ai.GetStackCount() == 0)
        {
            ai.SwitchState(new SearchState());
        }
    }
}
