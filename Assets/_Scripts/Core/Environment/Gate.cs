using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    [SerializeField] int offSet = 1;
    [SerializeField] Cube cubePrefab;

    [Header("DEPENDENCIES")]
    public Image image;
    public MeshRenderer renderer;

    [Header("DEBUG")]
    public bool isFilling;
    public bool isLocked;
    public float fillSpeed;

    private Tween fillTween;
    private Tween unFillTween;

    private IGateable gateable;

    private void Fill()
    {
        unFillTween.Kill(false);

        fillTween = DOTween.To(() => image.fillAmount, x => image.fillAmount = x, 1f, fillSpeed).SetSpeedBased();

        fillTween
            .OnStart(delegate
            {
                renderer.enabled = false;
            })
            .OnComplete(delegate
            {
                gateable.IncreaseStackCount(gateable.StackCount() * offSet, cubePrefab);
                gameObject.SetActive(false);
            })
            .Play();
    }

    public void UnFill()
    {
        fillTween.Kill(false);

        unFillTween = DOTween.To(() => image.fillAmount, x => image.fillAmount = x, 0f, fillSpeed).SetSpeedBased();

        unFillTween
            .OnStart(delegate
            {

            })
            .OnComplete(delegate
            {
                renderer.enabled = true;
            })
            .Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.CHARACTER) && !isLocked && (other.GetComponent<IGateable>() != null))
        {
            isLocked = true;
            isFilling = true;

            gateable = other.GetComponent<IGateable>();

            Fill();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.CHARACTER) && other.GetComponent<IGateable>() == gateable)
        {
            isLocked = false;
            isFilling = false;

            gateable = null;

            UnFill();
        }
    }
}
