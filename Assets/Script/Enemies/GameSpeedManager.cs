using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    private float[] speeds = { 1f, 2f, 4f, 8f, 16f };
    private int currentIndex = 0;

    public void ToggleSpeed()
    {
        currentIndex = (currentIndex + 1) % speeds.Length;
        Time.timeScale = speeds[currentIndex];
    }

    public float CurrentSpeed => speeds[currentIndex];
}
