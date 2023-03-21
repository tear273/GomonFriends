using System;
using UnityEngine;
using UnityEngine.UI;

public class PedometerDemo2 : MonoBehaviour
{

	#region Fields
	private PedometerPlugin _pedometerPlugin;
	private string _demoName = "[PedometerDemo] ";
	private SensorUtilsPlugin _sensorUtilsPlugin;
	public Text hasStepDetectorStatusText;
	public Text prevTotalStepCountText;
	public Text totalStepCountText;
	public Text stepYesterdayCountText;
	public Text stepTodayCountText;
	public Text stepByDateText;
	#endregion

	#region Methods
	// Use this for initialization
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

		//check if step detector is supported
		bool hasStepCounter = _sensorUtilsPlugin.HasStepCounter ();
		if (hasStepCounter) {
			UpdateStepDetectorStatus ("available");
			// event listeners
			AddEventListeners ();
		} else {
			UpdateStepDetectorStatus ("not available");
			Debug.LogWarning("Step Counter on current device is not available!");
		}
	}

	private void OnDestroy ()
	{
		RemoveEventListeners ();
	}

	private void AddEventListeners ()
	{
		//set call back listener for pedometer events
		_pedometerPlugin.OnLoadTotalStepCount += OnLoadTotalStepCount;
		_pedometerPlugin.OnLoadPrevStepCount += OnLoadPrevStepCount;
		_pedometerPlugin.OnLoadTotalStepToday += OnLoadTotalStepToday;
	}

	private void RemoveEventListeners ()
	{
		//set call back listener for pedometer events
		_pedometerPlugin.OnLoadTotalStepCount -= OnLoadTotalStepCount;
		_pedometerPlugin.OnLoadPrevStepCount -= OnLoadPrevStepCount;
		_pedometerPlugin.OnLoadTotalStepToday -= OnLoadTotalStepToday;
	}

	public void LoadSteps ()
	{
		// test only remove this after testing because usually on 1st run there's no save step
		// from yesteday this value is for testing previos total steps
		// you just need to call this once on 1st run after that remove it or comment it
		// to avoid messing around the step save
		//pedometerPlugin.SetStepYesterday( 1500 );

		_pedometerPlugin.LoadPrevTotalStep ();
		_pedometerPlugin.LoadTotalStep ();
		_pedometerPlugin.LoadStepToday ();

		UpdateYesterdayStepCount (_pedometerPlugin.GetStepYesterday ());
		// get step on this date
		// for testing my current date now is april 8,2017 i want to get step yesterday so minus 1 day
		// so i put 3 , 7, 2017 - change this to your current date for testing
		// remember month start's with 0 so jan  = 0, that's why april is 3
		GetStepByDate (3, 7, 2017);
	}

	public void DeleteData ()
	{
		_pedometerPlugin.DeleteData ();
		LoadSteps ();
	}

	// get step on specific date if the step is available and save if not zero
	public void GetStepByDate (int month, int day, int year)
	{
		// if there's no step save return 0 here
		int stepCount = _pedometerPlugin.GetStepByDate (month, day, year);
		Debug.Log (_demoName + "get step by date stepCount: " + stepCount + " on " + month + "/" + day + "/" + year);

		UpdateStepCountByDate (stepCount);
	}


	// for updating text ui
	private void UpdateStepDetectorStatus (string val)
	{
		if (hasStepDetectorStatusText != null) {
			hasStepDetectorStatusText.text = String.Format ("Status: {0}", val);
		}
	}

	private void UpdatePrevStepCount (int stepCount)
	{
		if (prevTotalStepCountText != null) {
			prevTotalStepCountText.text = String.Format ("Prev Step: {0}", stepCount);
		}
	}

	// for updating text ui
	private void UpdateTotalStepCount (int stepCount)
	{
		if (totalStepCountText != null) {
			totalStepCountText.text = String.Format ("Total Step: {0}", stepCount);
		}
	}

	private void UpdateStepTodayCount (int stepCount)
	{
		if (stepTodayCountText != null) {
			stepTodayCountText.text = String.Format ("Today Step: {0}", stepCount);
		}
	}

	private void UpdateYesterdayStepCount (int stepCount)
	{
		if (stepYesterdayCountText != null) {
			stepYesterdayCountText.text = String.Format ("Step yesterday: {0}", stepCount);
		}
	}

	private void UpdateStepCountByDate (int stepCount)
	{
		if (stepByDateText != null) {
			stepByDateText.text = String.Format ("Step by date: {0}", stepCount);
		}
	}
	#endregion

	#region Events
	private void OnLoadTotalStepCount (int stepCount)
	{
		UpdateTotalStepCount (stepCount);
		Debug.Log (_demoName + "OnLoadTotalStepCount count " + stepCount);
	}

	private void OnLoadPrevStepCount (int stepCount)
	{		
		UpdatePrevStepCount (stepCount);
		Debug.Log (_demoName + "OnLoadPrevStepCount count " + stepCount);
	}

	private void OnLoadTotalStepToday (int stepCountToday)
	{
		UpdateStepTodayCount (stepCountToday);
	}
	#endregion
}