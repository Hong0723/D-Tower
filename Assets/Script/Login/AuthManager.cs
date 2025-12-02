using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static string Nickname { get; private set; }

    public static void SetNickname(string nickname)
    {
        Nickname = nickname;
    }

    public static string GetNickname()
    {
        return Nickname;
    }

    public async void SignUp(string email, string password)
    {
        try
        {
            var userCredential = await FirebaseInit.auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseInit.user = userCredential.User;
            Debug.Log("회원가입 성공");
        }
        catch (System.Exception e)
        {
            Debug.LogError("회원가입 실패: " + e.Message);
        }
    }

    public async void SignIn(string email, string password)
    {
        try
        {
            var userCredential = await FirebaseInit.auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseInit.user = userCredential.User;
            Debug.Log("로그인 성공");

            // Firebase DB / Firestore 에서 닉네임 가져오는 부분을 넣어야 함
            // AuthManager.SetNickname(불러온_닉네임);
        }
        catch (System.Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
        }
    }

    public void SignOut()
    {
        FirebaseInit.auth.SignOut();
        FirebaseInit.user = null;
        Nickname = null;
        Debug.Log("로그아웃 완료");
    }
}
