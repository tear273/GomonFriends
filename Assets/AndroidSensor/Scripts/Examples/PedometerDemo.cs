using System;
using UnityEngine;
using UnityEngine.UI;

public class PedometerDemo : MonoBehaviour
{
	#region Fields
	private PedometerPlugin _pedometerPlugin;
	private string _demoName = "[PedometerDemo] ";
	private SensorUtilsPlugin _sensorUtilsPlugin;
	public Text hasStepDetectorStatusText;
	public Text prevTotalStepCountText;
	public Text totalStepCountText;
	public Text stepTodayCountText;
	public Text stepDetectText;
	#endregion

	#region Methods
	void Start ()
	{
		// don't allow the device to sleep
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		//get the instance of pedometer plugin
		_pedometerPlugin = PedometerPlugin.GetInstance ();
		//initialize pedometer
		_pedometerPlugin.Init ();
		//set to zero to hide debug toast messages
		_pedometerPlugin.SetDebug (0);
		
		_sensorUtilsPlugin = SensorUtilsPlugin.GetInstance ();
		_sensorUtilsPlugin.Init();
		_sensorUtilsPlugin.SetDebug (0);

		//check if step counter is supported on the current android mobile device
		bool hasStepCounter = _sensorUtilsPlugin.HasStepCounter ();
		if (hasStepCounter) {
			// yehey your android mobile device support pedometer

			UpdateStepDetectorStatus ("available");
			// event listeners
			AddEventListeners ();
		} else {
			// if you get this meaning
			// pedometer is not available on your android mobile device sorry!
			UpdateStepDetectorStatus ("not available");
			Debug.LogWarning("Step Counter on current device is not available!");
		}
	}

	private void OnDestroy ()
	{
		RemoveEventListeners ();
	}

	// for listening on pedometer events
	private void AddEventListeners ()
	{
		//set call back listener for pedometer events
		_pedometerPlugin.OnLoadTotalStepCount += OnLoadTotalStepCount;
		_pedometerPlugin.OnLoadPrevStepCount += OnLoadPrevStepCount;
		_pedometerPlugin.OnLoadTotalStepToday += OnLoadTotalStepToday;
		_pedometerPlugin.OnStepCount += OnStepCount;
		_pedometerPlugin.OnStepCountToday += OnStepCountToday;
		_pedometerPlugin.OnStepDetect += OnStepDetect;
	}

	// for listening on pedometer events
	private void RemoveEventListeners ()
	{
		//set call back listener for pedometer events
		_pedometerPlugin.OnLoadTotalStepCount -= OnLoadTotalStepCount;
		_pedometerPlugin.OnLoadPrevStepCount -= OnLoadPrevStepCount;
		_pedometerPlugin.OnLoadTotalStepToday -= OnLoadTotalStepToday;
		_pedometerPlugin.OnStepCount -= OnStepCount;
		_pedometerPlugin.OnStepCountToday -= OnStepCountToday;
		_pedometerPlugin.OnStepDetect -= OnStepDetect;
	}

	// the pedometer service is not auto start
	// call this to start the service
	// and don't worry after you close or quit the unity3d application the
	// pedometer service will start and run again
	public void StartPedometerService ()
	{
		string serviceNotificationName = "MyAwesomePedometerService";
		string serviceNotificationBodyText = "running...";
		// here you start and pass the sensor delay that you want to use
		_pedometerPlugin.StartPedometerService (SensorDelay.SENSOR_DELAY_FASTEST,serviceNotificationName,serviceNotificationBodyText);
		UpdateStepDetectorStatus ("Service Started");
		Debug.Log (_demoName + "StartPedometerService has been called");
	}

	// call this to stop the pedometer service
	public void StopPedometerService ()
	{
		_pedometerPlugin.StopPedometerService ();
		UpdateStepDetectorStatus ("Service Stopped");
	}

	// for loading steps
	public void LoadSteps ()
	{
		_pedometerPlugin.LoadPrevTotalStep ();
		_pedometerPlugin.LoadTotalStep ();
		_pedometerPlugin.LoadStepToday ();
	}

	// get step on specific date if the step is available and save if not zero
	public void GetStepByDate (int month, int day, int year)
	{
		int stepCount = _pedometerPlugin.GetStepByDate (month, day, year);
		Debug.Log (_demoName + "stepCount: " + stepCount + " on " + month + "/" + day + "/" + year);
	}
	
	//for updating the demo text ui
	private void UpdateStepDetectorStatus (string val)
	{
		if (hasStepDetectorStatusText != null) {
			hasStepDetectorStatusText.text = String.Format ("Status: {0}", val);
		}
	}

	private void UpdatePrevStepCount (int totalPrevStepCount)
	{
		if (prevTotalStepCountText != null) {
			prevTotalStepCountText.text = String.Format ("Prev Step: {0}", totalPrevStepCount);
		}
	}

	private void UpdateTotalStepCount (int count)
	{
		if (totalStepCountText != null) {
			totalStepCountText.text = String.Format ("Total Step: {0}", count);
		}
	}

	private void UpdateStepTodayCount (int stepsToday)
	{
		if (stepTodayCountText != null) {
			stepTodayCountText.text = String.Format ("Today Step: {0}", stepsToday);
		}
	}

	private void UpdateStepDetect (string status)
	{
		if (stepDetectText != null) {
			stepDetectText.text = String.Format ("Step Detect: {0}", status);
		}
	}
	#endregion

	#region Events
	private void OnStepCountToday (int totalStepToday)
	{
		UpdateStepTodayCount (totalStepToday);
	}

	private void OnLoadTotalStepCount (int totalStepCount)
	{
		UpdateTotalStepCount (totalStepCount);
		Debug.Log (_demoName + "OnLoadTotalStepCount count " + totalStepCount);
	}

	private void OnLoadPrevStepCount (int totalStepCount)
	{		
		UpdatePrevStepCount (totalStepCount);
		Debug.Log (_demoName + "OnLoadPrevStepCount count " + totalStepCount);
	}

	private void OnLoadTotalStepToday (int stepCountToday)
	{
		UpdateStepTodayCount (stepCountToday);
	}

	//step detect event is triggered
	private void OnStepDetect ()
	{
		UpdateStepDetect ("Ok!");
		Debug.Log (_demoName + "OnStepDetect");
	}

	//step count event is triggered
	private void OnStepCount (int totalStepCount)
	{
		Debug.Log (_demoName + "OnStepCount count " + totalStepCount);
		// for updating total step count
		UpdateTotalStepCount (totalStepCount);
	}
	#endregion
}