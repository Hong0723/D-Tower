using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameSpeedManager : MonoBehaviour
{
    public static GameSpeedManager I;

    [SerializeField] private float[] speeds = { 1f, 2f, 4f, 8f, 16f };
    private int currentIndex = 0;
    private float savedTimeScale = 1f;

    public event Action<float> OnSpeedChanged;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        // 저장된 배속 불러오기
        currentIndex = PlayerPrefs.GetInt("SpeedIndex", 0);
        savedTimeScale = speeds[currentIndex];

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySpeed();  // 씬 이동 후 속도 복원
    }

    private void Start()
    {
        ApplySpeed();
    }

    public void ToggleSpeed()
    {
        currentIndex = (currentIndex + 1) % speeds.Length;

        PlayerPrefs.SetInt("SpeedIndex", currentIndex);
        PlayerPrefs.Save();

        ApplySpeed();
    }

    private void ApplySpeed()
    {
        float speed = speeds[currentIndex];
        Time.timeScale = speed;
        savedTimeScale = speed;

        OnSpeedChanged?.Invoke(speed); // UI 업데이트
    }

    public void PauseGame()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = savedTimeScale;
        OnSpeedChanged?.Invoke(savedTimeScale); // UI 즉시 반영
    }

    public float CurrentSpeed => speeds[currentIndex];
}
