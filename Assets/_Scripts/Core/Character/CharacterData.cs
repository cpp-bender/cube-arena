using UnityEngine;

[CreateAssetMenu(menuName ="Cube Arena/Character Data", fileName ="Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] ColorType colorType;
    [SerializeField] Material material;

    public ColorType ColorType { get => colorType; }
    public Material Material { get => material; }
}
