using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cube Arena/Cube Points Data", fileName = "Cube Points Data")]
public class CubePointsData : ScriptableObject
{
    [SerializeField] List<Vector3> points;

    public List<Vector3> Points { get => points; set => points = value; }
}
