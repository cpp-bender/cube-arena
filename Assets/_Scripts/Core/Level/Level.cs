using UnityEngine;

[CreateAssetMenu(menuName = "Cube Arena/Level", fileName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] Cube cubePrefab;
    [SerializeField] int startCubeCount;
    [SerializeField] int totalCubeCount;

    public Cube CubePrefab { get => cubePrefab; }
    public int TotalCubeCount { get => totalCubeCount; }
    public int StartCubeCount { get => startCubeCount; }
}
