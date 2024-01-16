using System.Collections;
using DG.Tweening;
using UnityEngine;

public class InvestState : AIBaseState
{
    private Vector3 closestInvestPoint;

    public InvestState()
    {
        name = AIState.Invest;
    }

    public override IEnumerator Enter(AIController ai)
    {
        canUpdate = true;

        yield return RotateTween().Play().WaitForCompletion();

        closestInvestPoint = FindClosestInvestPoint();

        Tween RotateTween()
        {
            var investPoint = FindClosestInvestPoint();

            var dir = (investPoint - ai.transform.position).normalized;

            var lookRot = Quaternion.LookRotation(dir);

            Tween rotateTween = ai.transform.DORotate(lookRot.eulerAngles, .25f)
                .OnStart(delegate
                {

                });

            return rotateTween;
        }

        Vector3 FindClosestInvestPoint()
        {
            Vector3 closestInvestPoint = Vector3.zero;

            var col = InvestManager.Instance.GetComponent<CapsuleCollider>();

            return col.ClosestPoint(ai.transform.position);
        }

        yield return null;
    }

    public override void Update(AIController ai)
    {
        if (!canUpdate) return;

        Check();

        Move();

        void Check()
        {
            if (ai.GetStackCount() == 0)
            {
                ai.SwitchState(new DecideState());
            }
        }

        void Move()
        {
            var threshold = .5f;

            var dist = (closestInvestPoint - ai.transform.position).sqrMagnitude;

            var dir = (closestInvestPoint - ai.transform.position).normalized;

            ai.transform.Translate(dir * Time.deltaTime * ai.aIData.MoveSpeed, Space.World);

            if (dist <= threshold)
            {
                ai.SwitchState(new OnInvestingState());
            }
        }
    }
}
