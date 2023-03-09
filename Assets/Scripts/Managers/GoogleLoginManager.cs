using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleLoginManager : Singletone<GoogleLoginManager>
{
   public async Task<string> GoogleServiceLogin()
    {
        TaskCompletionSource<string> task = new TaskCompletionSource<string>();

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => {

                if (success)
                {
                    string idToken = Social.localUser.userName;
                    Debug.Log($"{idToken} Goolge Play Login Success");
                    task.SetResult(idToken);

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
    //    ((PlayGamesPlatform)Social.Active).SignOut();
      //  Debug.Log("Logout Success");
#endif

    }
}
