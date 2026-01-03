// UIManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nicknameInput;
    public AuthManager authManager;

    public void OnClickSignUp()
    {
        string nickname = nicknameInput != null ? nicknameInput.text : "Player";
        authManager.SignUp(emailInput.text, passwordInput.text, nickname);
    }

    public async void OnClickSignIn()
    {
        bool success = await authManager.SignIn(emailInput.text, passwordInput.text);
        if (success)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("로그인에 실패했습니다.");
        }
    }

    public void OnClickSignOut()
    {
        authManager.SignOut();
    }
}