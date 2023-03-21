using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using BackendData.Base;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    //게임 정보 관리 데이터만 모아놓은 클래스
    public class BackendGameData
    {
        public readonly BackendData.GameData.UserData UserData = new();
        public readonly BackendData.GameData.DecoData DecoData = new();
        public readonly BackendData.GameData.FriendsData FriendsData = new();
        public readonly BackendData.GameData.SoundData SoundData = new();

        public readonly Dictionary<string, GameData> GameDataList = new Dictionary<string, GameData>();
        public BackendGameData()
        {
            GameDataList.Add("1", UserData);
            GameDataList.Add("2", DecoData);
            GameDataList.Add("3", FriendsData);
            GameDataList.Add("4", SoundData);
            /* GameDataList.Add("4", ProfileData);
             GameDataList.Add("5", ShopData);
             GameDataList.Add("6", MartData);
             GameDataList.Add("7", AnimalData);
             GameDataList.Add("8", PetData);
             GameDataList.Add("9", PartTimeData);
             GameDataList.Add("10", QuestData);
             GameDataList.Add("11", WeatherData);
             GameDataList.Add("12", DeliveryData);*/
        }
    }

    public BackendGameData backendGameData = new();

    public bool isLoading = false;
    public void Initialize()
    {
        //StaticManager.UI.SetLoading(true);

        BackendCustomSetting settings = new BackendCustomSetting();
        settings.clientAppID = "456fada0-be63-11ed-a6ad-89a4354c31564603";
        settings.signatureKey = "456fd4b0-be63-11ed-a6ad-89a4354c31564603";
        settings.isAllPlatform = true;
        settings.sendLogReport = true;
        settings.timeOutSec = 30;
        settings.useAsyncPoll = true;
        var callback = Backend.Initialize(settings);

        if (callback.IsClientRequestFailError())
        {
            Debug.LogError("네트워크에 연결이 되어 있지 않습니다.");
            StaticManager.UI.SetLoading(false);
            //StaticManager.UI.AlertUI.OpenUI("Error","네트워크에 연결이 되어 있지 않습니다.", () => StaticManager.Instance.ChangeScene("1. Login"));
            StaticManager.UI.AlertUI.OpenUI("Error", "네트워크에 연결이 되어 있지 않습니다.");
        }
        else
        {
            if (callback.IsSuccess())
            {
                StaticManager.UI.SetLoading(false);
                Debug.LogWarning("뒤끝 초기화가 완료되었습니다.");
                CreateSendQueueMgr();
                SetErrorHandler();

                isLoading = true;

                StartCoroutine(StaticManager.Chart.Initialize());
            }
            else
            {
                Debug.LogError("네트워크에 연결이 되어 있지 않습니다.");
                StaticManager.UI.SetLoading(false);
                // StaticManager.UI.AlertUI.OpenUI("네트워크에 연결이 되어 있지 않습니다.", () => StaticManager.Instance.ChangeScene("1. Login"));
                StaticManager.UI.AlertUI.OpenUI("Error", "네트워크에 연결이 되어 있지 않습니다.");
            }
        }
    }

    #region 버전 확인 (모바일)
    public void GetVersionInfo()
    {
        SendQueue.Enqueue(Backend.Utils.GetLatestVersion, callback =>
        {
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("버전정보를 불러오는 데 실패하였습니다.\n" + callback);
               // LoginSceneManager.Instance.versionChecked = true;
                return;
            }

            var version = callback.GetReturnValuetoJSON()["version"].ToString();

            Version server = new Version(version);
            Version client = new Version(Application.version);

            var result = server.CompareTo(client);
            if (result == 0)
            {
                // 0 이면 두 버전이 일치
                //LoginSceneManager.Instance.versionChecked = true;
                return;
            }
            else if (result < 0)
            {
                // 0 미만이면 server 버전이 client 이전 버전
                // 검수를 넣었을 경우 여기에 해당된다.
                // ex) 검수버전 3.0.0, 라이브에 운용되고 있는 버전 2.0.0, 콘솔 버전 2.0.0
                //LoginSceneManager.Instance.versionChecked = true;
                return;
            }
            else
            {
                // 0보다 크면 server 버전이 client 이후 버전
                if (client == null)
                {
                    // 클라이언트가 null인 경우 예외처리
                    Debug.LogError("클라이언트 버전정보가 null 입니다.");
                    //LoginSceneManager.Instance.versionChecked = true;
                    return;
                }
            }

            // 버전 업데이트 팝업
           // StaticManager.UI.OpenUI("Prefabs/GameScene/UpdateUI", LoginSceneManager.Instance.loginUICanvas.transform);
            return;
        });
    }
    #endregion

    //로딩에서 할당할 뒤끝 정보 클래스 초기화
    public void InitGameData()
    {
        backendGameData = new();
    }

    //SendQueue를 관리해주는 SendQueue 매니저 생성
    private void CreateSendQueueMgr()
    {
        var obj = new GameObject();
        obj.name = "SendQueueMgr";
        obj.transform.SetParent(this.transform);
        obj.AddComponent<SendQueueMgr>();
    }

    void Update()
    {
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
            Backend.ErrorHandler.Poll();
        }
    }

    //모든 뒤끝 함수에서 에러 발생 시, 각 에러에 따라 호출해주는 핸들러
    private void SetErrorHandler()
    {
        Backend.ErrorHandler.InitializePoll(true);

        //서버 점검 에러 발생 시
        Backend.ErrorHandler.OnMaintenanceError = () =>
        {
            Debug.LogError("점검 에러 발생!");
            StaticManager.UI.AlertUI.OpenUI("Info", "현재 서버 점검중입니다.");
        };

        //403 에러 발생 시
        Backend.ErrorHandler.OnTooManyRequestError = () =>
        {
            StaticManager.UI.AlertUI.OpenUI("Error", "비정상적인 해동이 감지되었습니다.");
        };

        //액세스 토큰 만료 후 리프레시 토큰 실패 시
        Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () =>
        {
            StaticManager.UI.AlertUI.OpenUI("Error", "다른 기기에서 로그인이 감지되었습니다.");
        };
    }

    //업데이트가 발생한 이후에 호출에 대한 응답을 반환해주는 대리자 함수
    public delegate void AfterUpdateFunc(BackendReturnObject callback);

    //값이 바뀐 데이터가 있을 시 저장
    public void UpdateAllGameData(AfterUpdateFunc afterUpdateFunc)
    {
        string info = string.Empty;

        //바뀥 데이터가 몇 개 있는지 체크
        List<GameData> gameDatas = new List<GameData>();

        //if (gameDatas.Count == 0) return;

        foreach (var gameData in backendGameData.GameDataList)
        {
            if (ES3.Load<bool>("IsChangeData", gameData.Value.GetTableName() + ".es3"))
            {
                info += gameData.Value.GetTableName() + "\n";
                gameDatas.Add(gameData.Value);
            }
        }


        if (gameDatas.Count <= 0)
            afterUpdateFunc(null);


        else if (gameDatas.Count == 1)
        {
            //하나라면 찾아서 해당 테이블만 업데이트
            foreach (var gameData in gameDatas)
            {
                if (ES3.Load<bool>("IsChangeData", gameData.GetTableName() + ".es3"))
                {
                    gameData.Update(callback =>
                    {
                        //성공할 경우 데이터 변경 여부를 false로 변경
                        if (callback.IsSuccess())
                        {
                            Debug.LogError("저장 성공 ㅠㅠ");
                            ES3.Save("IsChangeData", false, gameData.GetTableName() + ".es3");
                            //gameData.IsChangedData = false;
                        }
                        else
                        {
                            Debug.LogError(callback.ToString() + "\n" + info);
                        }
                        Debug.LogError($"UpdateV2 : {callback}\n업데이트 테이블 : \n{info}");

                        if (afterUpdateFunc != null)
                            afterUpdateFunc(callback);  //지정한 대리자 함수 호출
                    });
                }

            }
        }

        else
        {
            //2개 이상이라면 트랜잭션에 묶어서 업데이트
            //단 10개 이상이면 트랜잭션 실패 주의
            List<TransactionValue> transactionList = new List<TransactionValue>();

            //변경된 데이터만큼 트랜잭션 추가
            foreach (var gameData in gameDatas)
            {
                transactionList.Add(gameData.GetTransactionValue());
            }

            SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, transactionList, callback =>
            {
                Debug.LogError($"Backend.BMember.TransactionWriteV2 : {callback}");

                if (callback.IsSuccess())
                {
                    foreach (var data in gameDatas)
                    {
                        Debug.LogError("저장 성공 ㅠㅠ");
                        ES3.Save("IsChangeData", false, data.GetTableName() + ".es3");
                        //data.IsChangedData = false;
                    }
                }
                else
                {
                    Debug.LogError(callback.ToString() + "\n" + info);
                }

                if (afterUpdateFunc != null)
                    afterUpdateFunc(callback);
            });
        }
    }

}
