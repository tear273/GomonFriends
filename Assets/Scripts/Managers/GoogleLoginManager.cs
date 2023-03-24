using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using System;
using BackEnd;

public class GoogleLoginManager : Singletone<GoogleLoginManager>
{

    private FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }


    public void GoogleLogin(Action<BackendReturnObject> func)
    {
#if UNITY_EDITOR
        //Backend.BMember.DeleteGuestInfo();
        SendQueue.Enqueue(Backend.BMember.GuestLogin, func);
        return;
#endif


        if (Social.localUser.authenticated)
        {
            var token = GetFederationToken();
            if (token.Equals(string.Empty))
            {
                //StaticManager.UI.SetLoading(false);

                Debug.LogError("GPGS 토큰이 존재하지 않습니다.");
               // StaticManager.UI.AlertUI.OpenUI("Error","GPGS 토큰이 존재하지 않습니다.");
                return;
            }
            Debug.Log("Token" + token);
            SendQueue.Enqueue(Backend.BMember.AuthorizeFederation, token, FederationType.Google, func);
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    var token = GetFederationToken();
                    if (token.Equals(string.Empty))
                    {
                       // StaticManager.UI.SetLoading(false);

                        Debug.LogError("GPGS 토큰이 존재하지 않습니다.");
                       // StaticManager.UI.AlertUI.OpenUI("Error","GPGS 토큰이 존재하지 않습니다.");
                        return;
                    }
                    //Backend.BMember.AuthorizeFederation(token, FederationType.Google, func);
                    Debug.Log("Token" + token);
                    SendQueue.Enqueue(Backend.BMember.AuthorizeFederation, token, FederationType.Google, func);
                }
                else
                {
                   // StaticManager.UI.SetLoading(false);
                    Debug.LogError("GPGS 토큰이 존재하지 않습니다2.");
                  //  StaticManager.UI.AlertUI.OpenUI("Error","GPGS 토큰이 존재하지 않습니다2.\n" + success.ToString());
                }
            });
        }
    }

    private string GetFederationToken()
    {
#if UNITY_ANDROID
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Debug.LogError("GPGS에 접속되어 있지 않습니다.");
            return string.Empty;
        }

        string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
        return _IDtoken;
#endif

        return null;
    }

    

    public void GoogleLogout()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
        }
#endif

    }
}
