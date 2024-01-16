using UnityEngine;

[CreateAssetMenu(menuName = "Cube Arena/Player Data", fileName = "Player Data")]
public class PlayerData : CharacterData
{
    [SerializeField] float speed = 7.5f;

    public float Speed { get => speed; }
}
