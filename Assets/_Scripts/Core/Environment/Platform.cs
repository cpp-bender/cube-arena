using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : SingletonMonoBehaviour<Platform>
{
    [Header("DEPENDENCIES")]
    [SerializeField] InputData input;
    [SerializeField] float turnSpeed;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        var turnDirection = CalculateTurnDirection();

        Turn(turnDirection);
    }

    private Vector3 CalculateTurnDirection()
    {
        return Vector3.up * input.Horizontal;
    }

    private void Turn(Vector3 direction)
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + direction, Time.deltaTime * turnSpeed);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
