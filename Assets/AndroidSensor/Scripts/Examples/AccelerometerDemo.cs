using System;
using UnityEngine;
using UnityEngine.UI;

public class AccelerometerDemo : MonoBehaviour {

	#region Fields
	private SensorUtilsPlugin _sensorUtilsPlugin;
	private ShakePlugin _shakePlugin;
	public Text xText;
	public Text yText;
	public Text zText;
	public Text sensitivityText;
	public Slider sensitivitySlider;
	public Text delayUpdateText;
	public Slider delayUpdateSlider;
	#endregion

	#region Methods
	// Use this for initialization
	void Start (){
		// don't allow the device to sleep
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		_sensorUtilsPlugin = SensorUtilsPlugin.GetInstance ();
		_sensorUtilsPlugin.Init();
		_sensorUtilsPlugin.SetDebug (0);
		
		_shakePlugin = ShakePlugin.GetInstance();
		if (_sensorUtilsPlugin.HasAccelerometer())
		{
			_shakePlugin.Init(OnShake, OnAccelerometer);
			_shakePlugin.SetDebug(0);
			_shakePlugin.RegisterSensorListener(SensorDelay.SENSOR_DELAY_NORMAL);
		}else
		{
			Debug.LogWarning("Accelerometer on current device is not available!");	
		}

		SetSensitivitySlider();
		SetDelayUpdateSlider();
	}

	private void SetSensitivitySlider(){
		int sensitivity = (int)sensitivitySlider.value;
		UpdateSensitivity(sensitivity);
		Debug.Log("CheckSensitivitySlider value: " + sensitivity);
	}

	private void SetDelayUpdateSlider(){
		int delayUpdate = (int)delayUpdateSlider.value;
		UpdateDelayUpdate(delayUpdate);
		Debug.Log("CheckDelayUpdateSlider value: " + delayUpdate);
	}

	public void OnSensitivitySliderChange(){
		SetSensitivitySlider();
	}

	public void OnDelayUpdateSliderChange(){
		SetDelayUpdateSlider();
	}

	private void OnApplicationPause(bool val){
		if(val){
			if(_shakePlugin!=null){
				_shakePlugin.RemoveSensorListener();
			}
		}else{
			if(_shakePlugin!=null){
				_shakePlugin.RegisterSensorListener(SensorDelay.SENSOR_DELAY_NORMAL);
			}
		}
	}
	private void UpdateSensitivity(int sensitivity){
		if(_shakePlugin!=null){
			_shakePlugin.SetSensitivity(sensitivity);
			if(sensitivityText!=null){
				sensitivityText.text = String.Format("Sensitivity: {0}",sensitivity);
			}
		}
	}

	private void UpdateDelayUpdate(int delayUpdate){
		if(_shakePlugin!=null){
			_shakePlugin.SetDelayUpdate(delayUpdate);
			if(delayUpdateText!=null){
				delayUpdateText.text = String.Format("Delay Update: {0}",delayUpdate);
			}
		}
	}


	private void UpdateAccelerometerUIValues(string x, string y, string z)
	{
		if (xText!=null)
		{
			xText.text = $"X: {x}";
		}
				
		if (yText!=null)
		{
			yText.text = $"Y: {y}";
		}
				
		if (zText!=null)
		{
			zText.text = $"Z: {z}";
		}
	}
	#endregion

	#region Events
	private void OnShake(int count, float speed){
		
	}
	private void OnAccelerometer(string sensorData){
		if (!string.IsNullOrEmpty(sensorData))
		{
			// convert data
			AccelerometerData accelerometerData = JsonUtility.FromJson<AccelerometerData>(sensorData);
			if (accelerometerData!=null)
			{
				UpdateAccelerometerUIValues(
					accelerometerData.x.ToString(),
					accelerometerData.y.ToString(),
					accelerometerData.z.ToString()
					);
			}
			else
			{
				UpdateAccelerometerUIValues("no data","no data","no data");
			}
		}
		else
		{
			UpdateAccelerometerUIValues("no data","no data","no data");
		}
	}
	#endregion
}
