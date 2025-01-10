using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public readonly int hashMove = Animator.StringToHash("Move");

    public float speed = 5f;
    public float rotateSpeed = 100f;

    private Animator animator;
    private Rigidbody rb;
    private PlayerInput input;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var targetVelocity = input.Move * speed;
        var pos = transform.position;
        pos += targetVelocity * Time.deltaTime;
        rb.MovePosition(pos);
        animator.SetFloat(hashMove, input.Move.x == 0 ? input.Move.z : input.Move.x);
    }
}
