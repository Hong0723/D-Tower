using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();

        var hb = Instantiate(health.HealthbarPrefab, transform);
        hb.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        health.AttachHealthbar(hb);
    }
}
