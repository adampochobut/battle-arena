using System.Collections;
using UnityEngine;

public class AthenaShieldAbility : MonoBehaviour
{
    public GameObject shieldPrefab;

    [Header("Input")]
    public KeyCode abilityKey = KeyCode.R;

    [Header("Timing")]
    public float duration = 3f;
    public float cooldown = 6f;

    private bool _onCooldown = false;
    private GameObject _activeShield;

    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && !_onCooldown)
            StartCoroutine(ShieldRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        _onCooldown = true;

        // Spawn shield centered on the character
        if (shieldPrefab != null)
        {
            _activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            _activeShield.transform.SetParent(transform, true);
            _activeShield.transform.localPosition = Vector3.zero; // follows the character
        }

        yield return new WaitForSeconds(duration);

        if (_activeShield != null)
            Destroy(_activeShield);

        yield return new WaitForSeconds(cooldown);
        _onCooldown = false;
    }
}

