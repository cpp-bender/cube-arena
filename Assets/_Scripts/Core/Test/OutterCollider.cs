using UnityEngine;

public class OutterCollider : MonoBehaviour
{
    [Range(0f, 1f)] public float range;

    private const float DISTANCE = 4.25F;

    private void OnDrawGizmos()
    {
        float angRad = range * 2 * Mathf.PI;

        Vector3 coords = AngToDir(angRad) * DISTANCE;

        Gizmos.DrawSphere(coords * DISTANCE, 1f);

        Vector3 AngToDir(float angRad)
        {
            float x = Mathf.Cos(angRad);
            float z = Mathf.Sin(angRad);
            return new Vector3(x, 0f, z);
        }
    }

    public Vector3 GetOutterPoint(float range)
    {
        float angRad = range * 2 * Mathf.PI;

        Vector3 coords = AngToDir(angRad) * DISTANCE;

        return coords * DISTANCE;

        Vector3 AngToDir(float angRad)
        {
            float x = Mathf.Cos(angRad);
            float z = Mathf.Sin(angRad);
            return new Vector3(x, 0f, z);
        }
    }
}
