using System.Collections;
using UnityEngine;

public class SearchState : AIBaseState
{
    public SearchState()
    {
        name = AIState.Search;
    }

    public override IEnumerator Enter(AIController ai)
    {
        if (ai.GetStackCount() == 0)
        {
            ai.SwitchAnimation("Idle");
        }

        var cube = FindClosestCube();

        if (cube != null)
        {
            ai.SwitchState(new CollectState(cube));
        }

        else if (cube == null)
        {
            ai.SwitchState(new SearchState());
        }

        Cube FindClosestCube()
        {
            Cube closestCube = null;

            var cubes = GameObject.FindObjectsOfType<Cube>();

            float maxDist = Mathf.Infinity;

            foreach (var cube in cubes)
            {
                float dist = (cube.transform.position - ai.transform.position).sqrMagnitude;

                if (dist < maxDist && cube.CanCollectable() && cube.targetAI == null)
                {
                    closestCube = cube;
                    maxDist = dist;
                }
            }

            return closestCube;
        }

        yield return null;
    }

    public override void Update(AIController ai)
    {

    }
}
