using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;
    public float height = 15f;

    void LateUpdate()
    {
        if (!target) return;

        transform.position = target.position + new Vector3(0, height, 0);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
