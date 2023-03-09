using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class BackendManager : Singletone<BackendManager>
{
    Action loginAction = null;


    public void StartBackend()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }
    }

    public void CustomSignUp(string id, string pw)
    {

        Debug.Log("회원가입을 요청합니다.");

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            CustomLogin(id, pw,loginAction);
            Debug.Log("회원가입에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("회원가입에 실패했습니다. : " + bro);
        }
        // Step 2. 회원가입 구현하기 로직
    }

    public void CustomLogin(string id, string pw, Action action)
    {
        // Step 3. 로그인 구현하기 로직
        loginAction = action;
        Debug.Log("로그인을 요청합니다.");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인이 성공했습니다. : " + bro);
            loginAction();
            loginAction = null;
        }
        else
        {
            CustomSignUp(id, pw);
            Debug.LogError("로그인이 실패했습니다. : " + bro);
        }
    }


    public void UpdateNickname(string nickname, Action func)
    {
        // Step 4. 닉네임 변경 구현하기 로직
        Debug.Log("닉네임 변경을 요청합니다.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            func();
            Debug.Log("닉네임 변경에 성공했습니다 : " + bro);
        }
        else
        {
            Debug.LogError("닉네임 변경에 실패했습니다 : " + bro);
        }
    }
}
