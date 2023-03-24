using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private UIButton exitGame_btn;

    private void Awake()
    {
        //StaticManager가 없을 경우 새로 생성
        if (FindObjectOfType(typeof(StaticManager)) == null)
        {
            var obj = Resources.Load<GameObject>("Manager/StaticManager");
            Instantiate(obj);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Initialize("");
#elif UNITY_ANDROID
        RequestPermission();
#endif

    }

    void RequestPermission()
    {
        var pluginClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = pluginClass.GetStatic<AndroidJavaObject>("currentActivity");
        unityActivity.Call("RequestPermission");
    }

    void Initialize(string temp)
    {
        StaticManager.UI.alertUI = NGUITools.AddChild(NGUITools.GetRoot(gameObject), StaticManager.UI.origin_ComonPopup).GetComponent<Common_Popup>();
        StaticManager.UI.alertUI.gameObject.SetActive(false);
        
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

        _event = new EventDelegate(OnClickExit_Btn);
        exitGame_btn.onClick.Add(_event);
    }

    void OnClickExit_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        question_popup.gameObject.SetActive(true);
        question_popup.Init(() => {
            StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
            Application.Quit();
        }, "게임을 나가시겠습니까?");
    }

    void OnClickGoogleLogin_btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        GoogleLoginManager.Instance.GoogleLogin(AuthorizeProcess);
        /*if(googleLogin != null)
        {
            StopCoroutine(googleLogin);
            googleLogin = null;
        }
        googleLogin = GoogleLogin();
        StartCoroutine(googleLogin);*/
    }

    void AuthorizeProcess(BackendReturnObject callback)
    {

        //StaticManager.UI.SetLoading(false);
        Debug.LogWarning($"Backend.BMember.AuthroizeProcess : {callback}");

      

        

        //새로 가입인 경우에는 StatusCode가 201, 기존 로그인일 경우에는 200이 리턴
        if (callback.GetStatusCode() == "201")
        {
            terms_view.gameObject.SetActive(true);
        }
        else
        {
            //닉네임이 없을 경우
            if (string.IsNullOrEmpty(Backend.UserNickName))
            {
                createNickName_view.gameObject.SetActive(true);
                return;
            }

            gameStartView.gameObject.SetActive(true);
            //로딩 불러오기
//            StartCoroutine(LoginSceneManager.Instance.InitializeLoading());
        }


    }


    void OnClickAgree_btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        terms_view.gameObject.SetActive(false);
        createNickName_view.gameObject.SetActive(true);
    }

    void OnClickGameStart_btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        StartCoroutine(SceneInitalized());
        //SceneManager.LoadScene(1);
    }


    IEnumerator SceneInitalized() {
        //트랜잭션으로 불러온 후, 안불러질 경우 각자 Get 함수로 불러오는 함수
        initializeStep.Enqueue(() => { TransactionRead(NextStep); });

        //뒤끝 데이터 초기화
        StaticManager.Backend.InitGameData();

        //Queue에 저장된 함수 순차적으로 실행
        NextStep(true, string.Empty);
        
        yield return null;
        //StaticManager.Sound.Initalized();
    }

    void OnClickLogout_btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        question_popup.gameObject.SetActive(true);
        question_popup.Init(() => {
            StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
            gameStartView.gameObject.SetActive(false);
            GoogleLoginManager.Instance.GoogleLogout();
            question_popup.gameObject.SetActive(false);
        },"로그아웃 하시겠습니까?\n로그아웃이 완료되면 첫 화면으로 돌아갑니다.");
    }

    void OnClickConfirem_Btn()
    {
        StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
        if (createNickName_view.nickTextField.Checked_NickName())
        {
            var nickname = createNickName_view.nickTextField.GetInput_TextField().value;
            SendQueue.Enqueue(Backend.BMember.UpdateNickname, nickname, callback =>
            {
                try
                {
                  //  StaticManager.UI.SetLoading(false);
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError(callback);

                        if (callback.GetStatusCode() == "400")
                        {
                            if (callback.GetMessage().Contains("undefined nickname"))
                            {
                                StaticManager.UI.AlertUI.OpenUI("Error", "닉네임이 비어 있습니다.");
                                // errorText.text = StaticManager.Chart.Langauge.Localize(3);
                            }
                            else if (callback.GetMessage().Contains("bad beginning or end"))
                            {
                                StaticManager.UI.AlertUI.OpenUI("Error", "닉네임에 비속어가 포함되어 있습니다.");
                                // errorText.text = StaticManager.Chart.Langauge.Localize(4);
                            }
                            else
                            {
                                //errorText.text = "알 수 없는 에러입니다.";
                            }
                        }
                        else if (callback.GetStatusCode() == "409")
                        {
                            StaticManager.UI.AlertUI.OpenUI("Error", "중복된 닉네임 입니다.");
                            //errorText.text = StaticManager.Chart.Langauge.Localize(6);
                        }
                        else
                        {
                            StaticManager.UI.AlertUI.OpenUI("Error",callback.ToString());
                        }

                       // errorText.gameObject.GetComponent<DOTweenAnimation>().DORestart();
                    }
                    else
                    {
                        Debug.LogError("닉네임 생성 성공");
                        createNickName_view.gameObject.SetActive(false);
                        gameStartView.gameObject.SetActive(true);


                        //최초 계정 생성
                        // LoginSceneManager.Instance.isFirst = true;
                        //로딩 불러오기
                        //StartCoroutine(LoginSceneManager.Instance.InitializeLoading());
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    StaticManager.UI.AlertUI.OpenUI("Error",e.ToString());
                }
            });
        }
        else
        {

        }
    }


    //각 뒤끝 함수를 호출하는 BackendGameDataLoad에서 실행한 결과를 처리하는 함수
    //성공하면 다음 스텝으로 이동, 실패하면 에러 UI 띄움
    private void NextStep(bool isSuccess, string errorInfo)
    {
        if (isSuccess)
        {
            //currentLoadingCount++;

            if (initializeStep.Count > 0)
                initializeStep.Dequeue().Invoke();
            else
                SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError(errorInfo);
            StaticManager.UI.AlertUI.OpenUI("Error",errorInfo);
            
        }
    }

    private delegate void BackendLoadStep();

    private readonly Queue<BackendLoadStep> initializeStep = new Queue<BackendLoadStep>();

    //트랜잭션 읽기 함수
    private void TransactionRead(BackendData.Base.Normal.AfterBackendLoadFunc func)
    {
        bool isSuccess = false;
        string errorInfo = String.Empty;

        //트랜잭션 리스트 생성
        List<TransactionValue> transactionList = new List<TransactionValue>();

        //게임 테이블 데이터만큼 트랜잭션 불러오기
        foreach (var gameData in StaticManager.Backend.backendGameData.GameDataList)
        {
            transactionList.Add(gameData.Value.GetTransactionValue());
        }

        if (transactionList.Count > 0)
        {
            //트랜잭션으로 데이터를 찾지 못하여 에러가 발생 시 개별로 GetMyData 호출
            foreach (var gameData in StaticManager.Backend.backendGameData.GameDataList)
            {
                initializeStep.Enqueue(() =>
                {
                  //  ShowDataName(gameData.Key);
                    gameData.Value.BackendGameDataLoad(NextStep);
                });

               // maxLoadingCount++;
            }

            isSuccess = true;
            func.Invoke(isSuccess, errorInfo);
        }
        else
        {
            SendQueue.Enqueue(Backend.GameData.TransactionReadV2, transactionList, callback =>
            {
                //데이터를 모두 불러왔을 경우
                if (callback.IsSuccess())
                {
                    JsonData gameDataJson = callback.GetFlattenJSON()["Responses"];

                    int index = 0;

                    foreach (var gameData in StaticManager.Backend.backendGameData.GameDataList)
                    {
                        initializeStep.Enqueue(() =>
                        {
                            //ShowDataName(gameData.Key);

                            //불러온 데이터를 로컬에서 파싱
                            gameData.Value.BackendGameDataLoadByTransaction(gameDataJson[index++], NextStep);
                        });

                       // maxLoadingCount++;
                    }
                    
                    isSuccess = true;
                }
                else
                {
                    errorInfo = callback.ToString();
                }

                func.Invoke(isSuccess, errorInfo);
            });
        }
    }



}
