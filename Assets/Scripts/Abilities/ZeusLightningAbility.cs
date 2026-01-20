using System.Collections;
using UnityEngine;

public class ZeusLightningAbility : MonoBehaviour
{
    public GameObject lightningMarkerPrefab;
    public GameObject lightningStrikePrefab;

    public float strikeDelay = 0.7f;
    public float cooldown = 2f;
    public KeyCode abilityKey = KeyCode.Q;

    public LayerMask groundLayers = ~0;

    bool _onCooldown;

    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && !_onCooldown)
            Cast();
    }

    void Cast()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayers))
            StartCoroutine(LightningRoutine(hit.point));
    }

    IEnumerator LightningRoutine(Vector3 pos)
    {
        _onCooldown = true;

        GameObject marker = null;
        if (lightningMarkerPrefab != null)
            marker = Instantiate(lightningMarkerPrefab, pos + Vector3.up * 0.02f, Quaternion.identity);

        yield return new WaitForSeconds(strikeDelay);

        if (marker != null) Destroy(marker);

        if (lightningStrikePrefab != null)
        {
            var strike = Instantiate(lightningStrikePrefab, pos, Quaternion.identity);
            Destroy(strike, 0.3f);
        }

        yield return new WaitForSeconds(cooldown);
        _onCooldown = false;
    }
}
