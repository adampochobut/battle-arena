using System.Collections;
using UnityEngine;

public class AresSwordSwarm : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject swordPrefab;

    [Header("Spawn")]
    public int swordCount = 5;
    public float spawnRadius = 1.5f;

    [Header("Charge")]
    public float chargeSpeed = 8f;
    public float chargeDuration = 1.25f;
    public float randomTargetRadius = 8f;

    [Header("Input")]
    public KeyCode abilityKey = KeyCode.R;

    private bool _onCooldown = false;
    public float cooldown = 2f;

    void Update()
    {
        if (_onCooldown) return;

        if (Input.GetKeyDown(abilityKey))
        {
            StartCoroutine(SwarmRoutine());
        }
    }

    IEnumerator SwarmRoutine()
    {
        _onCooldown = true;

        // pick a random target point on the ground near the spawner
        Vector3 randomOffset = new Vector3(
            Random.Range(-randomTargetRadius, randomTargetRadius),
            0f,
            Random.Range(-randomTargetRadius, randomTargetRadius)
        );
        Vector3 targetPoint = transform.position + randomOffset;

        // spawn swords in a circle
        for (int i = 0; i < swordCount; i++)
        {
            float angle = i * (360f / swordCount);
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            Vector3 spawnPos = transform.position + dir * spawnRadius + Vector3.up * 0.5f;

            GameObject sword = Instantiate(swordPrefab, spawnPos, Quaternion.identity);

            // face the target point
            Vector3 lookDir = (targetPoint - spawnPos);
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.001f)
                sword.transform.rotation = Quaternion.LookRotation(lookDir);

            // launch it
            StartCoroutine(ChargeSword(sword, targetPoint));
        }

        // cooldown
        yield return new WaitForSeconds(cooldown);
        _onCooldown = false;
    }

    IEnumerator ChargeSword(GameObject sword, Vector3 targetPoint)
    {
        float t = 0f;

        while (t < chargeDuration && sword != null)
        {
            t += Time.deltaTime;

            Vector3 dir = (targetPoint - sword.transform.position);
            dir.y = 0f;

            if (dir.sqrMagnitude < 0.05f)
                break;

            sword.transform.position += dir.normalized * chargeSpeed * Time.deltaTime;
            yield return null;
        }

        if (sword != null)
            Destroy(sword);
    }
}
