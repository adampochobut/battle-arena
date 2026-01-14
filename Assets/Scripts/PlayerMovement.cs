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

    [Header("Attack")]
    [SerializeField] float attackSpeed = 1.5f;
    [SerializeField] float attackDelay = 0.3f;
    [SerializeField] float attackDistance = 1.5f;
    [SerializeField] int attackDamage = 1;

    bool playerBusy = false;
    Interactable target;

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
            if(hit.transform.CompareTag("Interactable"))
            {
                target = hit.transform.GetComponent<Interactable>();
                if(clickEffect != null)
                { Instantiate(clickEffect, hit.transform.position + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation); }
            }
            else{
                agent.destination = hit.point;

            if (clickEffect != null)
                Instantiate(clickEffect, hit.point + Vector3.up * 0.1f, clickEffect.transform.rotation);
            }
        }
    }

    private void Update()
    {
        FollowTarget();
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

    void FollowTarget()
    {
        if(target == null) return;

        if(Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
        { ReachDistance(); }
        else
        { agent.SetDestination(target.transform.position); }
    }

    void ReachDistance()
    {
        agent.SetDestination(transform.position);

        if(playerBusy) return;

        playerBusy = true;

        Invoke(nameof(SendAttack), attackDelay);
        Invoke(nameof(ResetBusyState), attackSpeed);
    }

    void SendAttack()
    {
        if(target == null) return;

        if(target.myActor.currentHealth <= 0)
        { target = null; return; }

        //Instantiate(hitEffect, target.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        target.GetComponent<Actor>().TakeDamage(attackDamage);
    }

    void ResetBusyState()
    {
        playerBusy = false;
    }

}
