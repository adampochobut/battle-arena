using UnityEngine;
using Unity.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5f;

    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        //rotates player movement so it feels natural in isometric
        Vector3 inputVector = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 isoDirection = Quaternion.Euler(0, -45, 0) * inputVector;


        if (isoDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(isoDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }

        rb.MovePosition(rb.position + isoDirection * speed * Time.fixedDeltaTime);

        //prevents infinite momentum buildup
        rb.linearVelocity = Vector2.zero;

    }
}
