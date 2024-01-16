using UnityEngine;

public class InvestTriggerArea : MonoBehaviour
{
    [SerializeField] InvestManager InvestManager;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(Tags.CHARACTER))
        {
            EventManager.Instance.CharacterTriggerStayInvestArea(other.attachedRigidbody.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.CHARACTER))
        {
            EventManager.Instance.CharacterTriggerExitInvestArea(other.attachedRigidbody.gameObject);
        }
    }
}
