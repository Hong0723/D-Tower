using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image fill;

    private Transform target;
    private float maxHP;
    private Vector3 originalScale;

    void Start()
    {
        // HP바의 기본 스케일 저장
        originalScale = transform.localScale;
    }

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


    void LateUpdate()
    {
        if (target == null) return;

        // Enemy 위 고정
        transform.position = target.position + new Vector3(0, 0.7f, 0);

        // 회전 고정(카메라 방향 유지)
        transform.rotation = Camera.main.transform.rotation;
    }
}
