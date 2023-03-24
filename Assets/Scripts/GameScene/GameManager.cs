using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameManager : Singletone<GameManager>
{
    private PedometerPlugin _pedometerPlugin;
    private SensorUtilsPlugin _sensorUtilsPlugin;


	
	public UILabel todayPedometor_Label;

	[SerializeField]
	UIButton friendsConfiguration_btn;
	[SerializeField]
	UIButton option_btn;
	[SerializeField]
	UIButton decoration_btn;

	[SerializeField]
	Friends_View frend_View;
	[SerializeField]
	FManagerManet_View friendsManageMend_View;
	[SerializeField]
	FriendsPurchase_Popup friendsPurchase_Popup;
	[SerializeField]
	Option_Popup option_Popup;
	[SerializeField]
	Decoration_View decoration_View;
	[SerializeField]
	UIWidget numContainer;

	[SerializeField]
	UILabel friendsNum_Label;
	[SerializeField]
	UILabel friendsShip_Label;
	[SerializeField]
	UILabel ganet_Label;
	[SerializeField]
	UIButton friendShip_Button;
	[SerializeField]
	ChangeStartPopup changeStartPopup;
	[SerializeField]
	UIButton store_btn;

	[SerializeField]
	List<GameObject> deco;
	[SerializeField]
	List<GameObject> friends;


	public List<GameObject> Friends => friends;
	public List<GameObject> Deco => deco;
	public FManagerManet_View FrendsManageMent_View => friendsManageMend_View;
	public FriendsPurchase_Popup FriendsPurchase_Popup => friendsPurchase_Popup;
	public UILabel Ganet_Label => ganet_Label;
	public UILabel FriendsShip_Label => friendsShip_Label;
	public UIWidget NumContainer => numContainer;
	public float orgin_NimContainer_Y;
	public PedometerPlugin PedometerPlugin => _pedometerPlugin;


	private void Start()
    {
		Initalize();
    }

    void Initalize()
    {
		StaticManager.UI.alertUI = NGUITools.AddChild(NGUITools.GetRoot(FrendsManageMent_View.gameObject), StaticManager.UI.origin_ComonPopup).GetComponent<Common_Popup>();
		StaticManager.UI.alertUI.gameObject.SetActive(false);

		StaticManager.Sound.Initalized();

		InitPedometor();
		AddEvents();
		SetDeco();
		VetaVersionPopup();
		//SetInitData();
		StartCoroutine(InitData());
	}

	void VetaVersionPopup()
    {
        if (StaticManager.Backend.backendGameData.UserData.New)
        {
			StaticManager.UI.alertUI.OpenUI("Info", "본 게임은 사전 체험판으로 정기적으로 업데이트 됩니다!");
			StaticManager.Backend.backendGameData.UserData.SetNew(false);
			StaticManager.Backend.backendGameData.UserData.Update((callback) => { });
		}
		
    }

	void SetDeco()
    {
		Dictionary<string,bool> items = StaticManager.Backend.backendGameData.DecoData.Deco;

		foreach(string key in items.Keys)
        {
			var _deco = deco.Find(value => value.name.Equals(key));

			if(_deco != null)
            {
				_deco.SetActive(items[key]);
            }
        }
	}

	IEnumerator InitData()
    {
		yield return new WaitForEndOfFrame();
		friendsShip_Label.text = StaticManager.Backend.backendGameData.UserData.FriendShipStar.ToString();
		ganet_Label.text = StaticManager.Backend.backendGameData.UserData.Ganet.ToString();

		
	}

	void AddEvents()
    {
		EventDelegate _event = new EventDelegate(OnClickOption_Btn);
		option_btn.onClick.Add(_event);

		_event = new EventDelegate(OnClickDecoration_Btn);
		decoration_btn.onClick.Add(_event);

		_event = new EventDelegate(OnClickFriendShipStar_Btn);
		friendShip_Button.onClick.Add(_event);

		_event = new EventDelegate(OnClickFrendsConfiguration_Btn);
		friendsConfiguration_btn.onClick.Add(_event);

		_event = new EventDelegate(OnClickStore_Btn);
		store_btn.onClick.Add(_event);
	}

	void OnClickStore_Btn()
    {
		StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
		StaticManager.UI.alertUI.OpenUI("Info", "Comming Soon");
	}

	void OnClickFriendShipStar_Btn()
    {
		StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
		changeStartPopup.gameObject.SetActive(true);

	}

	void OnClickFrendsConfiguration_Btn()
    {
		StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
		frend_View.gameObject.SetActive(true);

	}

	void OnClickDecoration_Btn()
    {
		StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
		decoration_View.gameObject.SetActive(true);

		orgin_NimContainer_Y = numContainer.transform.localPosition.y;

		Vector3 pos = numContainer.transform.position;
		pos.y = decoration_View.Close_Btn.transform.position.y;
		numContainer.transform.position = pos;

		pos = numContainer.transform.localPosition;
		pos.y += decoration_View.Close_Btn.GetComponent<UIWidget>().height / 2 + numContainer.height / 2;
		numContainer.transform.localPosition = pos;

	}

    void InitPedometor()
    {
		// don't allow the device to sleep
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		//get the instance of pedometer plugin
		_pedometerPlugin = PedometerPlugin.GetInstance();
		//initialize pedometer
		_pedometerPlugin.Init();
		//set to zero to hide debug toast messages
		_pedometerPlugin.SetDebug(0);

		_sensorUtilsPlugin = SensorUtilsPlugin.GetInstance();
		_sensorUtilsPlugin.Init();
		_sensorUtilsPlugin.SetDebug(0);

		
		
		//check if step counter is supported on the current android mobile device
		bool hasStepCounter = _sensorUtilsPlugin.HasStepCounter();
		if (hasStepCounter)
		{
			// yehey your android mobile device support pedometer

			//UpdateStepDetectorStatus("available");
			// event listeners
			AddEventListeners();
		}
		else
		{
			// if you get this meaning
			// pedometer is not available on your android mobile device sorry!
			//UpdateStepDetectorStatus("not available");
			Debug.LogWarning("Step Counter on current device is not available!");
		}

		

		StartCoroutine( Steps());
		//StartCoroutine(Steps());
	}

	IEnumerator Steps()
    {
		StartPedometerService();
		yield return new WaitForSeconds(0.2f);
		LoadSteps();
	}

	private void OnDestroy()
	{
		RemoveEventListeners();
	}

	// for listening on pedometer events
	private void RemoveEventListeners()
	{
		//set call back listener for pedometer events
		_pedometerPlugin.OnLoadTotalStepCount -= OnLoadTotalStepCount;
		_pedometerPlugin.OnLoadPrevStepCount -= OnLoadPrevStepCount;
		_pedometerPlugin.OnLoadTotalStepToday -= OnLoadTotalStepToday;
		_pedometerPlugin.OnStepCount -= OnStepCount;
		_pedometerPlugin.OnStepCountToday -= OnStepCountToday;
		_pedometerPlugin.OnStepDetect -= OnStepDetect;
	}

	private void AddEventListeners()
	{
		//set call back listener for pedometer events
		_pedometerPlugin.OnLoadTotalStepCount += OnLoadTotalStepCount;
		_pedometerPlugin.OnLoadPrevStepCount += OnLoadPrevStepCount;
		_pedometerPlugin.OnLoadTotalStepToday += OnLoadTotalStepToday;
		_pedometerPlugin.OnStepCount += OnStepCount;
		_pedometerPlugin.OnStepCountToday += OnStepCountToday;
		_pedometerPlugin.OnStepDetect += OnStepDetect;


		
	}

	private void OnLoadTotalStepCount(int totalStepCount)
	{
		print("OnLoadTotalStepCount = " + totalStepCount);
		if(StaticManager.Backend.backendGameData.UserData.Pedometor == 0)
        {
			StaticManager.Backend.backendGameData.UserData.SetPedometor(totalStepCount);
			StaticManager.Backend.backendGameData.UserData.Update((callback) =>
			{
                if (callback.IsSuccess())
                {
					StartCoroutine(SetText(totalStepCount  -  (StaticManager.Backend.backendGameData.UserData.PurchaseFriendShipStar + StaticManager.Backend.backendGameData.UserData.Pedometor)));
				}
                else
                {
					Debug.LogError("걸음수 저장 실패");
					StaticManager.Backend.backendGameData.UserData.Pedometor = 0;
					_pedometerPlugin.LoadTotalStep();
				}
			});

        }
        else
        {
			StartCoroutine(SetText(totalStepCount - (StaticManager.Backend.backendGameData.UserData.PurchaseFriendShipStar + StaticManager.Backend.backendGameData.UserData.Pedometor)));
		}


		

	}

	private void OnLoadPrevStepCount(int totalStepCount)
	{
		print("OnLoadPrevStepCount = " + totalStepCount);
	}

	private void OnStepDetect()
	{
		
	}


	public void LoadSteps()
	{
		_pedometerPlugin.LoadPrevTotalStep();
		_pedometerPlugin.LoadTotalStep();
		_pedometerPlugin.LoadStepToday();

		//_pedometerPlugin.LoadPrevTotalStep();
		//_pedometerPlugin.LoadTotalStep();
		//todayPedometor_Label.text = _pedometerPlugin.GetTotalStepToday().ToString();
	}

	private void OnLoadTotalStepToday(int stepCountToday)
	{
		print("OnLoadTotalStepToday = " + stepCountToday);
		//StartCoroutine(SetText(stepCountToday));

		print("OnLoadTotalStepTodayEnd = " + stepCountToday);
	}

	private void OnStepCountToday(int totalStepToday)
	{
		print("OnStepCountToday = " + totalStepToday);
		
	}

	IEnumerator SetText(int stepsToday)
    {
		yield return new WaitForSeconds(0.2f);

		Debug.Log("StartCorutine");
		string step = new StringBuilder().Append(stepsToday).ToString();
		todayPedometor_Label.text = step;
		option_Popup.TotalWalk = new StringBuilder().Append(stepsToday + StaticManager.Backend.backendGameData.UserData.PurchaseFriendShipStar).ToString();
		changeStartPopup.WalkCount = stepsToday;
	}


	private void OnStepCount(int totalStepToday)
	{
		print("OnStepCount = " + totalStepToday);
		StartCoroutine(SetText(totalStepToday - (StaticManager.Backend.backendGameData.UserData.PurchaseFriendShipStar + StaticManager.Backend.backendGameData.UserData.Pedometor)));
		//todayPedometor_Label.text = totalStepToday.ToString();
		print("OnStepCountEnd = " + totalStepToday);
	}

	public void StartPedometerService()
	{
		Debug.Log("StartService");
		string serviceNotificationName = "MyAwesomePedometerService";
		string serviceNotificationBodyText = "running...";
		// here you start and pass the sensor delay that you want to use
		_pedometerPlugin.StartPedometerService(SensorDelay.SENSOR_DELAY_FASTEST, serviceNotificationName, serviceNotificationBodyText);
		//UpdateStepDetectorStatus("Service Started");
		//Debug.Log(_demoName + "StartPedometerService has been called");
	}



	public void ShowPurchasePopup(PurchasePopup_Info info)
    {
		friendsPurchase_Popup.Info = info.info;
		friendsPurchase_Popup.Price = info.price;
		friendsPurchase_Popup.MoneyType = info.moneyType;
		friendsPurchase_Popup.func = info.func;
		friendsPurchase_Popup.Image = info.thumbnail;
		friendsPurchase_Popup.gameObject.SetActive(true);
	}

	void OnClickOption_Btn()
    {
		StaticManager.Sound.PlaySounds(SoundsType.BUTTON);
		option_Popup.TotalWalk = todayPedometor_Label.text;

		option_Popup.gameObject.SetActive(true);

	}

	public void CalGanet(int value)
    {
		

		

		var cal = StaticManager.Backend.backendGameData.UserData.Ganet += value;
		StaticManager.Backend.backendGameData.UserData.SetGanet(cal);
		StaticManager.Backend.backendGameData.UserData.Update((callback) =>
		{

			ganet_Label.text = StaticManager.Backend.backendGameData.UserData.Ganet.ToString();
		});
		
	}

	public void CalFriendShipStar(int value)
	{
		StaticManager.Backend.backendGameData.UserData.FriendShipStar += value;
		StaticManager.Backend.backendGameData.UserData.SetFriendShipStar(StaticManager.Backend.backendGameData.UserData.FriendShipStar);
		friendsShip_Label.text = StaticManager.Backend.backendGameData.UserData.FriendShipStar.ToString();
	}




}
