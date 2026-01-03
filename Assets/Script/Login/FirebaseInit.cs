// FirebaseInit.cs
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseAuth auth;
    public static FirebaseUser user;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("FirebaseAuth Init Success");
            }
            else
            {
                Debug.LogError("FirebaseAuth Init Failed");
            }
        });
    }
}