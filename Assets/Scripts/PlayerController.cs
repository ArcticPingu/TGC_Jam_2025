using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Animator ani;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();

    }

    // Called only when the stick/keys change
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }

    // Called every physics frame, so movement is continuous
    void FixedUpdate()
    {
        // Apply movement input (force-based movement)
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 desiredVelocity = moveDirection * speed;

        // Use physics-based movement (not direct velocity set)
        rb.AddForce(desiredVelocity - rb.linearVelocity, ForceMode.VelocityChange);

        // Use actual physics velocity for animation
        Vector3 actualVelocity = rb.linearVelocity;
        ani.SetFloat("X", actualVelocity.x);
        ani.SetBool("idle", actualVelocity.magnitude < 0.15f);
    }


    void Update()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.y /= (Screen.width / Screen.height);

        Shader.SetGlobalVector("_PlayerPos", pos);
        Shader.SetGlobalVector("_test", transform.position);
    }
}
