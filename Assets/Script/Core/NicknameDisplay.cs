using UnityEngine;
using TMPro;

public class NicknameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nicknameText;

    private void Start()
    {
        string nickname = AuthManager.GetNickname();
        if (nicknameText != null)
            nicknameText.text = nickname;
    }
}