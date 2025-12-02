using UnityEngine;
using Firebase.Auth;
using Firebase.Database;

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

            Debug.Log("ȸ������ ����");
        }
        catch (System.Exception e)
        {
            Debug.LogError("ȸ������ ����: " + e.Message);
        }
    }

    public async void SignIn(string email, string password)
    {
        try
        {
            var userCredential = await FirebaseInit.auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseInit.user = userCredential.User;

            await LoadNickname(FirebaseInit.user.UserId);

            Debug.Log("�α��� ����");

            // Firebase DB / Firestore ���� �г��� �������� �κ��� �־�� ��
            // AuthManager.SetNickname(�ҷ���_�г���);
        }
        catch (System.Exception e)
        {
            Debug.LogError("�α��� ����: " + e.Message);
        }
    }

    public void SignOut()
    {
        FirebaseInit.auth.SignOut();
        FirebaseInit.user = null;
        PlayerPrefs.DeleteKey("Nickname");
        Debug.Log("�α׾ƿ� �Ϸ�");
    }

    private async System.Threading.Tasks.Task SaveNickname(string userId, string nickname)
    {
        try
        {
            await dbReference.Child("users").Child(userId).Child("nickname").SetValueAsync(nickname);
            Debug.Log("�г��� ���� �Ϸ�: " + nickname);
        }
        catch (System.Exception e)
        {
            Debug.LogError("�г��� ���� ����: " + e.Message);
        }
    }

    private async System.Threading.Tasks.Task LoadNickname(string userId)
    {
        try
        {
            var snapshot = await dbReference.Child("users").Child(userId).Child("nickname").GetValueAsync();
            if (snapshot.Exists)
            {
                string nickname = snapshot.Value.ToString();
                PlayerPrefs.SetString("Nickname", nickname);
                PlayerPrefs.Save();
                Debug.Log("�г��� �ҷ�����: " + nickname);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("�г��� �ҷ����� ����: " + e.Message);
        }
    }

    public static string GetNickname()
    {
        return PlayerPrefs.GetString("Nickname", "Player");
    }
}