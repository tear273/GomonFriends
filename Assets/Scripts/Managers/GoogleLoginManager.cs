using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using System;

public class GoogleLoginManager : Singletone<GoogleLoginManager>
{

    private FirebaseAuth auth;


    public async Task<string> GoogleServiceLogin(Action func)
    {
        TaskCompletionSource<string> task = new TaskCompletionSource<string>();
        Debug.Log("GoogleServiceLogin");
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => {

                if (success)
                {

                    Debug.Log("StartFirebase Login");

#if UNITY_EDITOR
                    string idToken = Social.localUser.id;
                    Debug.Log(Social.localUser.userName);
                    //Debug.Log($"{idToken} Goolge Play Login Success");
                    task.SetResult(idToken);

#elif UNITY_ANDROID

                    StartCoroutine(TryFirebaseLogin(func));
                    /*string idToken = PlayGamesPlatform.Instance.GetIdToken();


                    Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
                    auth.SignInWithCredentialAsync(credential).ContinueWith(_task => {
                        if (_task.IsCanceled)
                        {
                            Debug.LogError("SignInWithCredentialAsync was canceled.");
                            return;
                        }
                        if (_task.IsFaulted)
                        {
                            Debug.LogError("SignInWithCredentialAsync encountered an error: " + _task.Exception);
                            return;
                        }
                        Debug.Log($"{idToken} Goolge Play Login Success");
                        task.SetResult(idToken);
                        Debug.Log("Success!");
                    });*/

#endif

                    /*   string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
                       Debug.Log($"{idToken} Goolge Play Login Success");
                       task.SetResult(idToken);*/

                }
                else
                {
                    Debug.Log("Fail");
                    task.SetResult("");
                }
                
            });

        }
        else
        {
            Debug.Log("Fail");
#if UNITY_EDITOR
            task.SetResult("Lerpz");
#elif UNITY_ANDROID
task.SetResult("");
#endif


        }

        return await task.Task;
    }

    public void GoogleLogout()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
            auth.SignOut(); // Firebase 로그아웃
        }
#endif

    }

    IEnumerator TryFirebaseLogin(Action func)
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();


        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
            func();
            Debug.Log("Success!");
        });
    }
}
