using UnityEngine;
using System;

public class GameSpeedManager : MonoBehaviour
{
    [SerializeField] private float[] speeds = { 1f, 2f, 4f, 8f, 16f };
    private int currentIndex = 0;

    public event Action<float> OnSpeedChanged;

    private void Start()
    {
        ApplySpeed();
    }

    public void ToggleSpeed()
    {
        currentIndex = (currentIndex + 1) % speeds.Length;
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        Time.timeScale = speeds[currentIndex];
        OnSpeedChanged?.Invoke(speeds[currentIndex]); // UI 업데이트 이벤트
    }

    public float CurrentSpeed => speeds[currentIndex];
}
