using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Called only when the stick/keys change
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Called every physics frame, so movement is continuous
    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.y) * speed;
        rb.linearVelocity = velocity;
    }
}
