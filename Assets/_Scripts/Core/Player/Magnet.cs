using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] float power = 10f;
    [SerializeField] CharacterController characterController;
    private List<Rigidbody> rbs = new List<Rigidbody>();

    private void FixedUpdate()
    {
        if (!characterController.IsCharacterReachMaxStackCount())
        {
            for (int i = 0; i < rbs.Count; i++)
            {
                if (rbs[i].CompareTag(Tags.CUBE))
                {
                    var direction = transform.position - rbs[i].transform.position;
                    rbs[i].AddForce(direction * power);
                }
                else
                {
                    rbs.RemoveAt(i);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.CUBE) && !characterController.IsCharacterReachMaxStackCount())
        {
            var rb = other.attachedRigidbody.GetComponent<Rigidbody>();
            rbs.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.CUBE))
        {
            var rb = other.attachedRigidbody.GetComponent<Rigidbody>();
            rbs.Remove(rb);
        }
    }
}
