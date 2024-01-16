using UnityEngine;

public class CubeTriggerArea : MonoBehaviour
{
    [SerializeField] Cube cube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.CHARACTER))
        {
            EventManager.Instance.CharacterTriggerEnterCube(cube, other.attachedRigidbody.gameObject);
        }

        if (other.CompareTag(Tags.OBSTACLE))
        {
            EventManager.Instance.ObstacleTriggerEnterCube(cube, cube.GetCollecter());
        }
    }
}
