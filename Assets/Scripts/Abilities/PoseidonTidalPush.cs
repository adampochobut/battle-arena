using System.Collections;
using UnityEngine;

public class PoseidonTidalPush : MonoBehaviour
{
    public GameObject wavePrefab;

    [Header("Input")]
    public KeyCode abilityKey = KeyCode.E;

    [Header("Settings")]
    public float cooldown = 2f;

    [Header("Wave")]
    public float waveSpeed = 10f;
    public float waveLifeTime = 1.0f;
    public float spawnDistance = 1.5f;
    public float groundY = 0.5f;

    private bool _onCooldown = false;

    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && !_onCooldown)
            StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        _onCooldown = true;

        Vector3 spawnPos = transform.position + transform.forward * spawnDistance;
        spawnPos.y = groundY;

        GameObject wave = Instantiate(wavePrefab, spawnPos, Quaternion.LookRotation(transform.forward));

        float t = 0f;
        while (t < waveLifeTime)
        {
            wave.transform.position += transform.forward * (waveSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(wave);

        yield return new WaitForSeconds(cooldown);
        _onCooldown = false;
    }
}

