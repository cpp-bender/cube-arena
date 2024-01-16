using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    public List<GameState> gameStates;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InvokeLevelStart()
    {
        CheckGameStates();

        foreach (var gameState in gameStates)
        {
            gameState.OnLevelStart?.Invoke();
        }
    }

    public void InvokeLevelEnd()
    {
        CheckGameStates();

        foreach (var gameState in gameStates)
        {
            gameState.OnLevelEnd?.Invoke();
        }
    }

    private void CheckGameStates()
    {
        foreach (var item in gameStates)
        {
            if (item == null)
            {
                Debug.LogWarning("Assign game states entities!");
            }
        }
    }
}
