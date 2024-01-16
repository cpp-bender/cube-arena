using UnityEngine;

[CreateAssetMenu(menuName = "Cube Arena/Input Data")]
public class InputData : ScriptableObject
{
    [SerializeField] float horizontal;
    [SerializeField] float vertical;
    [SerializeField] float magnitude;

    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float Vertical { get => vertical; set => vertical = value; }
    public float Magnitude { get => magnitude; set => magnitude = value; }
}
