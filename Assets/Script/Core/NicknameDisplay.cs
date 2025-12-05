// NicknameDisplay.cs
using UnityEngine;
using TMPro;

public class NicknameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text nicknameText;

    private void Start()
    {
        string nickname = AuthManager.GetNickname();
        Debug.Log("NicknameDisplay - 가져온 닉네임: " + nickname);  // 디버그 추가

        if (nicknameText != null)
        {
            nicknameText.text = nickname;
            Debug.Log("닉네임 표시 완료");
        }
        else
        {
            Debug.LogError("nicknameText가 연결되지 않았습니다!");  // null 체크
        }
    }
}