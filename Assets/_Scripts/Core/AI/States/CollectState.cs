using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CollectState : AIBaseState
{
    private Cube targetCube;

    public CollectState(Cube cube)
    {
        targetCube = cube;

        name = AIState.Collect;
    }

    public override IEnumerator Enter(AIController ai)
    {
        targetCube.targetAI = ai;

        canUpdate = true;

        yield return RotateTween().Play().WaitForCompletion();

        Tween RotateTween()
        {
            var dir = (targetCube.transform.position - ai.transform.position).normalized;

            var lookRot = Quaternion.LookRotation(dir);

            float dot = Mathf.Abs(Vector3.Dot(ai.transform.position.normalized, dir));

            float duration = 1 - dot;

            duration = Mathf.Clamp(duration, 1 - dot, .15f);

            Vector3 clampedRot = new Vector3(0f, lookRot.eulerAngles.y, 0f);

            Tween rotateTween = ai.transform.DORotate(clampedRot, duration)
                .SetLink(targetCube.gameObject)
                .OnStart(delegate
                {
                    if (ai.GetStackCount() == 0)
                    {
                        ai.SwitchAnimation("Run");
                    }
                    else if (ai.GetStackCount() >= 1)
                    {
                        ai.SwitchAnimation("Surf");
                    }
                });

            return rotateTween;
        }
    }

    public override void Update(AIController ai)
    {
        if (!canUpdate) return;

        CheckTarget();

        Move();

        void CheckTarget()
        {
            if (!targetCube.CanCollectable())
            {
                ai.SwitchState(new DecideState());
            }
        }

        void Move()
        {
            var dir = (targetCube.transform.position - ai.transform.position).normalized;

            ai.transform.Translate(dir * Time.deltaTime * ai.aIData.MoveSpeed, Space.World);
        }
    }
}
