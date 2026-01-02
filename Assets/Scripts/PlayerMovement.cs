using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls input;
    private NavMeshAgent agent;

    [Header("Movement")]
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private Camera mainCamera;

    private float lookRotationSpeed = 8f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        input = new PlayerControls();
        input.Player.Move.performed += OnClickPerformed;
    }

    private void OnEnable()
    {
        if (input != null)
            input.Enable();
    }

    private void OnDisable()
    {
        if (input != null)
            input.Disable();
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        ClickToMove(mousePos);
    }

    private void ClickToMove(Vector2 mousePos)
    {
        Debug.Log("WHY1");

        if (mainCamera == null || agent == null) return;

        Debug.Log("WHY2");

        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, clickableLayers))
        {
            agent.destination = hit.point;

            if (clickEffect != null)
                Instantiate(clickEffect, hit.point + Vector3.up * 0.1f, clickEffect.transform.rotation);
        }
    }

    private void Update()
    {
        FaceTarget();
    }

    private void FaceTarget()
    {
        if (!agent.hasPath) return;

        Vector3 direction = (agent.destination - transform.position).normalized;
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }
}
