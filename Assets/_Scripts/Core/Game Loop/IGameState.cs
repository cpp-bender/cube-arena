using UnityEngine;

public interface IGameState
{
    void OnLevelStart();
    void OnLevelEnd();
}
