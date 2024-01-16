using UnityEngine;
using System;

[DefaultExecutionOrder(-100)]
public class EventManager : SingletonMonoBehaviour<EventManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    //A Character trigger the cube
    public event Action<Cube, GameObject> OnCharacterTriggerEnterCube;
    public void CharacterTriggerEnterCube(Cube cube, GameObject characterGO)
    {
        OnCharacterTriggerEnterCube(cube, characterGO);
    }

    //A Character trigger the obstacle
    public event Action<Cube, CharacterController> OnObstacleTriggerEnterCube;
    public void ObstacleTriggerEnterCube(Cube cube, CharacterController characterController)
    {
        OnObstacleTriggerEnterCube(cube, characterController);
    }

    //A Character trigger the invest Area
    public event Action<GameObject> OnCharacterTriggerStayInvestArea;
    public void CharacterTriggerStayInvestArea(GameObject characterGO)
    {
        OnCharacterTriggerStayInvestArea(characterGO);
    }

    public event Action<GameObject> OnCharacterTriggerExitInvestArea;
    public void CharacterTriggerExitInvestArea(GameObject characterGO)
    {
        OnCharacterTriggerExitInvestArea(characterGO);
    }
}
