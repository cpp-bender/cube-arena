using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RandomMoveState : AIBaseState
{
    private Vector3 randomPos;

    public RandomMoveState()
    {
        name = AIState.RandomMove;
    }

    public override IEnumerator Enter(AIController ai)
    {
        HandleMovePos();

        canUpdate = true;

        void HandleMovePos()
        {
            RaycastHit hit;

            Vector3 tempRndPos = Vector3.zero;

            float dist = 5f;

            tempRndPos = ai.transform.position + Random.insideUnitSphere * dist;

            tempRndPos = new Vector3(tempRndPos.x, 0f, tempRndPos.z);

            Vector3 rayOrigin = ai.transform.position;

            Vector3 rayDir = (tempRndPos - rayOrigin).normalized;

            Physics.Raycast(rayOrigin, rayDir, out hit);

            if (!hit.collider.CompareTag(Tags.UNREACHABLE))
            {
                randomPos = tempRndPos;
                RotateTween().Play();
            }
            else
            {
                ai.SwitchState(new DecideState());
            }
        }

        Tween RotateTween()
        {
            var dir = (randomPos - ai.transform.position).normalized;

            var lookRot = Quaternion.LookRotation(dir);

            float dot = Mathf.Abs(Vector3.Dot(ai.transform.position.normalized, dir));

            float duration = 1 - dot;

            duration = Mathf.Clamp(duration, 1 - dot, .15f);

            Tween rotateTween = ai.transform.DORotate(lookRot.eulerAngles, duration)
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

        yield return null;
    }

    public override void Update(AIController ai)
    {
        if (!canUpdate) return;

        CheckIfMove();

        Move();

        void Move()
        {
            var dir = (randomPos - ai.transform.position).normalized;

            ai.transform.Translate(dir * Time.deltaTime * ai.aIData.MoveSpeed, Space.World);
        }

        void CheckIfMove()
        {
            float dist = (ai.transform.position - randomPos).sqrMagnitude;

            if (dist <= 1f)
            {
                ai.SwitchState(new DecideState());
            }
        }
    }
}
