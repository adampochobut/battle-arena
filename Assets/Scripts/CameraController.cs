using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 8f;
    public Vector3 offset;

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoomDistance = 3f;
    public float maxZoomDistance = 15f;

    private PlayerControls input;
    private float currentZoomDistance;

    private void Awake()
    {
        input = new PlayerControls();
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

    private void Start()
    {
        currentZoomDistance = offset.y;
        if (currentZoomDistance <= 0)
            currentZoomDistance = (minZoomDistance + maxZoomDistance) / 2f; // Default to middle if invalid
    }

    void Update()
    {
        if (target == null) return;

        // Handle zoom input
        HandleZoom();

        // Update offset with current zoom distance (affecting Y coordinate)
        Vector3 currentOffset = new Vector3(offset.x, currentZoomDistance, offset.z);

        Vector3 desiredPosition = new Vector3(target.position.x + currentOffset.x, target.position.y + currentOffset.y, target.position.z + currentOffset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private void HandleZoom()
    {


        Vector2 scrollValue = input.Player.Zoom.ReadValue<Vector2>();

        float scrollDelta = scrollValue.y;

        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            currentZoomDistance -= scrollDelta * zoomSpeed;

            currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
        }
    }

    private void OnDestroy()
    {
        if (input != null)
            input.Dispose();
    }
}