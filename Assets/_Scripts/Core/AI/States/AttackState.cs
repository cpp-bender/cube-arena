using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AttackState : AIBaseState
{
    private PlayerController player;

    private bool stopFollowing;
    private int tempInt;

    public AttackState()
    {
        stopFollowing = false;
        
        tempInt = 0;

        name = AIState.Attack;
    }

    public override IEnumerator Enter(AIController ai)
    {
        ai.isAttacking = true;

        player = GameObject.FindObjectOfType<PlayerController>();

        StartAttackTimer().Play();
        
        canUpdate = true;

        Tween StartAttackTimer()
        {
            Tween timerTween = DOTween.To(() => tempInt, x => tempInt = x, 1, ai.aIData.AttackTime)
            .OnComplete(() => stopFollowing = true);
            return timerTween;
        }

        yield return null;
    }

    public override void Update(AIController ai)
    {
        if (!canUpdate) return;

        CheckTimer();

        CheckPlayer();

        Rotate();

        Move();

        void CheckTimer()
        {
            if (stopFollowing)
            {
                ai.SwitchState(new DecideState());
            }
        }

        void CheckPlayer()
        {
            float threshold = 1f;
            float dist = (player.transform.position - ai.transform.position).sqrMagnitude;

            if (dist <= threshold)
            {
                ai.SwitchState(new DecideState());
            }
        }

        void Move()
        {
            var dir = (player.transform.position - ai.transform.position).normalized;

            ai.transform.Translate(dir * Time.deltaTime * ai.aIData.MoveSpeed, Space.World);
        }

        void Rotate()
        {
            var lookDir = (player.transform.position - ai.transform.position).normalized;

            ai.transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
