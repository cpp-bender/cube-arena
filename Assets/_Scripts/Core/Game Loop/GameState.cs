using UnityEngine.Events;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameState : MonoBehaviour
{
    public UnityEvent OnLevelStart;
    public UnityEvent OnLevelEnd;
}
