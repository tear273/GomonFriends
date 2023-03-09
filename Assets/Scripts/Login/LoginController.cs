using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 로그인 화면 컨트롤AddEvent()
*/


public class LoginController : MonoBehaviour
{
    [SerializeField]
    private UIButton googleLogin_btn;
    [SerializeField]
    private TermsAndConditions_View terms_view;
    [SerializeField]
    private CreateNickName_View createNickName_view;
    [SerializeField]
    private GameStartView gameStartView;
    [SerializeField]
    private Question_Popup question_popup;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        AddEvent();
    }

    void AddEvent()
    {

        //구글 로그인 버튼 이벤트 추가
        EventDelegate _event = new EventDelegate(OnClickGoogleLogin_btn);
        googleLogin_btn.onClick.Add(_event);

        //전체 동의버튼 이벤트 추가
        _event = new EventDelegate(OnClickAgree_btn);
        terms_view.GetAgree_Btn.onClick.Add(_event);

        //닉네임 체크 이벤트 추가
         _event = new EventDelegate(OnClickConfirem_Btn);
         createNickName_view.GetConfirem_btn.onClick.Add(_event);

        //게임 시작 이벤트 추가
        _event = new EventDelegate(OnClickGameStart_btn);
        gameStartView.GetGameStart_Btn.onClick.Add(_event);

        //로그아웃 이벤트 추가
        _event = new EventDelegate(OnClickLogout_btn);
        gameStartView.GetLogOut_Btn.onClick.Add(_event);
    }

    void OnClickGoogleLogin_btn()
    {
        terms_view.gameObject.SetActive(true);
    }

    void OnClickAgree_btn()
    {
        terms_view.gameObject.SetActive(false);
        createNickName_view.gameObject.SetActive(true);
    }

    void OnClickGameStart_btn()
    {

    }

    void OnClickLogout_btn()
    {
        question_popup.gameObject.SetActive(true);
        question_popup.Init(() => {
            gameStartView.gameObject.SetActive(false);
            question_popup.gameObject.SetActive(false);
        },"로그아웃 하시겠습니까?\n로그아웃이 완료되면 첫 화면으로 돌아갑니다.");
    }

    void OnClickConfirem_Btn()
    {
        if (createNickName_view.nickTextField.Checked_NickName())
        {
            createNickName_view.gameObject.SetActive(false);
            gameStartView.gameObject.SetActive(true);
        }
        else
        {

        }
    }

    
}
