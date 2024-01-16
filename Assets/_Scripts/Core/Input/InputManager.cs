using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputData input;
    [SerializeField] FloatingJoystick joystick;

    private void Update()
    {
        input.Horizontal = joystick.Horizontal;
        input.Vertical = joystick.Vertical;

        var vector2 = new Vector2(input.Vertical, input.Horizontal);
        input.Magnitude = vector2.magnitude;
    }
}
