using UnityEngine;

public class SceneBGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(bgmClip);
    }
}
