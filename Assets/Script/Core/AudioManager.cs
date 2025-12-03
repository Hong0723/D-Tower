using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [Header("BGM 오디오 소스")]
    public AudioSource bgmSource;
    public bool IsPlayingBGM => bgmSource.isPlaying;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;  // 같은 노래 중복 방지

        bgmSource.loop = loop;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
