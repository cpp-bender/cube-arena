using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cube Arena/AI Data", fileName = "AI Data")]
public class AIData : CharacterData
{
    [SerializeField, Range(0f, 10f)] float decideDuration = 1f;
    [SerializeField, Range(1f, 20f)] float moveSpeed = 5f;
    [SerializeField, Range(5f, 20f)] float attackTime = 5f;

    [Header("DECIDE BEHAVIOURS"), Space(5f)]
    [SerializeField, Range(0f, 1f)] float searchPercentage = .5f;
    [SerializeField, Range(0f, 1f)] float attackPercentage = .5f;
    [SerializeField, Range(0f, 1f)] float investPercentage = .5f;

    public AIBaseState GetNextRandomState(AIController ai)
    {
        List<AIBaseState> tempList = new List<AIBaseState>();

        AddAttackProbability();

        AddSearchProbability();

        AddInvestProbability();

        void AddAttackProbability()
        {
            var player = FindObjectOfType<PlayerController>();

            if (player.HasAtLeastOneStack() && ai.HasAtLeastOneStack() && ai.GetStackCount() > player.GetStackCount())
            {
                for (int i = 0; i < AttackPercentage * 10; i++)
                {
                    tempList.Add(new AttackState());
                }
            }
        }

        void AddSearchProbability()
        {
            for (int i = 0; i < SearchPercentage * 10; i++)
            {
                tempList.Add(new SearchState());
            }
        }

        void AddInvestProbability()
        {
            if (ai.HasAtLeastOneStack())
            {
                for (int i = 0; i < investPercentage * 10; i++)
                {
                    tempList.Add(new InvestState());
                }
            }
        }

        if (tempList.Count == 0)
        {
            return new SearchState();
        }

        int rnd = Random.Range(0, tempList.Count);

        return tempList[rnd];
    }
    public float MoveSpeed { get => moveSpeed; }
    public float DecideDuration { get => decideDuration; }
    public float SearchPercentage { get => searchPercentage; }
    public float AttackPercentage { get => attackPercentage; }
    public float AttackTime { get => attackTime;}
}
