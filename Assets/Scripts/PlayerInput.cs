using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string moveVerticalName = "Vertical";
    public static readonly string moveHorizontalName = "Horizontal";
    public static readonly string fireAxisName = "Fire1";

    public Vector3 Move { get; private set; }
    public bool Fire { get; private set; }


    private void Update()
    {
        Move = new Vector3(Input.GetAxis(moveHorizontalName), 0f, Input.GetAxis(moveVerticalName));
        if (Move.magnitude > 1f)
        {
            Move.Normalize();
        }
        Fire = Input.GetButton(fireAxisName);
    }
}
