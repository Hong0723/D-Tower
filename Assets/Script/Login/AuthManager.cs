// AuthManager.cs
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    private void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void SignUp(string email, string password, string nickname)
    {
        try
        {
            var userCredential = await FirebaseInit.auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseInit.user = userCredential.User;
            await SaveNickname(FirebaseInit.user.UserId, nickname);
            PlayerPrefs.SetString("Nickname", nickname);
            PlayerPrefs.Save();
            Debug.Log("회원가입 성공");
        }
        catch (System.Exception e)
        {
            Debug.LogError("회원가입 실패: " + e.Message);
        }
    }

    public async Task<bool> SignIn(string email, string password)
    {
        try
        {
            var userCredential = await FirebaseInit.auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseInit.user = userCredential.User;
            await LoadNickname(FirebaseInit.user.UserId);
            Debug.Log("로그인 성공");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
            return false;
        }
    }

    public void SignOut()
    {
        FirebaseInit.auth.SignOut();
        FirebaseInit.user = null;
        PlayerPrefs.DeleteKey("Nickname");
        Debug.Log("로그아웃 완료");
    }

    private async Task SaveNickname(string userId, string nickname)
    {
        try
        {
            await dbReference.Child("users").Child(userId).Child("nickname").SetValueAsync(nickname);
            Debug.Log("닉네임 저장 완료: " + nickname);
        }
        catch (System.Exception e)
        {
            Debug.LogError("닉네임 저장 실패: " + e.Message);
        }
    }

    private async Task LoadNickname(string userId)
    {
        Debug.Log("LoadNickname 시작 - userId: " + userId);  // 추가
        try
        {
            var snapshot = await dbReference.Child("users").Child(userId).Child("nickname").GetValueAsync();
            Debug.Log("snapshot.Exists: " + snapshot.Exists);  // 추가

            if (snapshot.Exists)
            {
                string nickname = snapshot.Value.ToString();
                PlayerPrefs.SetString("Nickname", nickname);
                PlayerPrefs.Save();
                Debug.Log("닉네임 불러오기: " + nickname);
            }
            else
            {
                Debug.Log("DB에 닉네임이 없습니다");  // 추가
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("닉네임 불러오기 실패: " + e.Message);
        }
    }

    public static string GetNickname()
    {
        return PlayerPrefs.GetString("Nickname", "Player");
    }
}