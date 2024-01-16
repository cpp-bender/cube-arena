using UnityEngine;

public class DynamicTrigger : MonoBehaviour
{
    [SerializeField] Transform characterModelTransform;
    [SerializeField] float scaleOffset = 0.8f;

    private void FixedUpdate()
    {
        SetLocaPositionY(characterModelTransform.localPosition.y);
    }

    public void RefreshDynamicSettings(int cubeCount)
    {
        SetScaleY(cubeCount);
    }

    private void SetScaleY(int cubeCount)
    {
        var localScale = transform.localScale;

        if (cubeCount == 0)
        {
            localScale.y = 1;
        }
        else
        {
            localScale.y = cubeCount * -scaleOffset;
        }

        transform.localScale = localScale;
    }

    private void SetLocaPositionY(float posY)
    {
        var localPos = transform.localPosition;
        localPos.y = posY;
        transform.localPosition = localPos;
    }
}
