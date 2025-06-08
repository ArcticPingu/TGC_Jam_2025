using System;
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
    [SerializeField] private Animator spriteAni;
    public bool canMove;
    public AudioClip[] footsteps;
    public AudioSource source;
    private int count;
    public Vector3 forceMove;
    public bool mowing;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Called only when the stick/keys change
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 desiredVelocity = moveDirection * speed;

        if (!canMove)
        {
            desiredVelocity = new Vector3(0, 0, 0);
        }

        // Use physics-based movement (not direct velocity set)
        if (forceMove == Vector3.zero)
        {
            rb.AddForce(desiredVelocity - rb.linearVelocity, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(forceMove - rb.linearVelocity, ForceMode.VelocityChange);
        }
        

        // Use actual physics velocity for animation
        Vector3 actualVelocity = rb.linearVelocity;
        spriteAni.SetFloat("X", actualVelocity.x);
        spriteAni.SetBool("idle", actualVelocity.magnitude < 0.15f);
    }


    void Update()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.y /= (Screen.width / Screen.height);

        Shader.SetGlobalVector("_PlayerPos", pos);
        Shader.SetGlobalVector("_test", transform.position);

        // if(mowing)

    }

    public void PlayStep()
    {
        source.PlayOneShot(footsteps[count++%footsteps.Length]);
    }


}
