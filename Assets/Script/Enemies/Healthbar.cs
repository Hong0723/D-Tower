using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image fill;
    private Transform target;
    private float maxHP;

    public bool IsConfigured => fill != null;

    public void Setup(Transform followTarget, float max, float current)
    {
        target = followTarget;
        maxHP = max;
        UpdateHealth(current, maxHP);
    }

    public void UpdateHealth(float current, float max)
    {
        fill.fillAmount = current / max;
    }

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + new Vector3(0, 0.7f, 0);
    }
}
