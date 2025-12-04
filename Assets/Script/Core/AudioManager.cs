using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [Header("BGM 오디오 소스")]
    public AudioSource bgmSource;
    public bool IsPlayingBGM => bgmSource.isPlaying;

    [Header("SFX 오디오 소스")]
    public AudioSource sfxSource;

    [Header("SFX 클립")]
    public AudioClip towerShootBasic;   // 1~4단계
    public AudioClip towerShootLv5;     // 5단계 전용
    public AudioClip towerPlace;        // 타워 설치음
    public AudioClip monsterDie;        // 몬스터 죽음음

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (!sfxSource)
            sfxSource = gameObject.AddComponent<AudioSource>(); // 자동 생성
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.loop = loop;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
