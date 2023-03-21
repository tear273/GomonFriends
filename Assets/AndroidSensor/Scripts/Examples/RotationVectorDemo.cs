using System;
using UnityEngine;
using UnityEngine.UI;

public class RotationVectorDemo : MonoBehaviour {

	#region Fields
	private SensorUtilsPlugin _sensorUtilsPlugin;
	private RotationVectorPlugin _rotationVectorPlugin;
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
		
		_rotationVectorPlugin = RotationVectorPlugin.GetInstance();
		if (_sensorUtilsPlugin.HasGyroscope())
		{
			_rotationVectorPlugin.Init( OnRotationVector);
			_rotationVectorPlugin.SetDebug(0);
			_rotationVectorPlugin.RegisterSensorListener(SensorDelay.SENSOR_DELAY_NORMAL);
		}else
		{
			Debug.LogWarning("Gyroscope on current device is not available!");	
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
			if(_rotationVectorPlugin!=null){
				_rotationVectorPlugin.RemoveSensorListener();
			}
		}else{
			if(_rotationVectorPlugin!=null){
				_rotationVectorPlugin.RegisterSensorListener(SensorDelay.SENSOR_DELAY_NORMAL);
			}
		}
	}
	private void UpdateSensitivity(int sensitivity){
		if(_rotationVectorPlugin!=null){
			_rotationVectorPlugin.SetSensitivity(sensitivity);
			if(sensitivityText!=null){
				sensitivityText.text = String.Format("Sensitivity: {0}",sensitivity);
			}
		}
	}

	private void UpdateDelayUpdate(int delayUpdate){
		if(_rotationVectorPlugin!=null){
			_rotationVectorPlugin.SetDelayUpdate(delayUpdate);
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
	private void OnRotationVector(string sensorData){
		if (!string.IsNullOrEmpty(sensorData))
		{
			// convert data
			GravityData gravityData = JsonUtility.FromJson<GravityData>(sensorData);
			if (gravityData!=null)
			{
				UpdateAccelerometerUIValues(
					gravityData.x.ToString(),
					gravityData.y.ToString(),
					gravityData.z.ToString()
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


