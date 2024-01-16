using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PigyBank : MonoBehaviour
{
    [Header("DEPENDENCIES")]
    [SerializeField] GameObject pigyBank;
    [SerializeField] GameObject pigyBankBroken;
    [SerializeField] float duration = 1f;

    [SerializeField] List<Rigidbody> moneyRbs;

    public void GameEnd(Transform winerTransform)
    {
        pigyBank.SetActive(false);
        pigyBankBroken.SetActive(true);

        for (int i = 0; i < moneyRbs.Count; i++)
        {
            moneyRbs[i].AddForce(Vector3.one * Random.Range(-1000, 1000), ForceMode.Acceleration);
            moneyRbs[i].transform.DOMove(winerTransform.position + Vector3.up, duration).SetDelay(1f).Play().SetEase(Ease.Linear);
            moneyRbs[i].transform.DOScale(Vector3.zero, 1f).SetDelay(1f).Play().SetEase(Ease.Linear).OnComplete(() => Destroy(moneyRbs[i].gameObject));
        }
    }
}
