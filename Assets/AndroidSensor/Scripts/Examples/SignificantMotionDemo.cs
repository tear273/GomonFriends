using UnityEngine;
using UnityEngine.UI;

public class SignificantMotionDemo : MonoBehaviour {

	#region Fields
	private SensorUtilsPlugin _sensorUtilsPlugin;
	private SignificantMotionPlugin _significantMotionPlugin;
	public Text statusText;
	#endregion

	#region Methods
	// Use this for initialization
	void Start (){
		// don't allow the device to sleep
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		_sensorUtilsPlugin = SensorUtilsPlugin.GetInstance ();
		_sensorUtilsPlugin.Init();
		_sensorUtilsPlugin.SetDebug (0);
		
		_significantMotionPlugin = SignificantMotionPlugin.GetInstance();
		if (_sensorUtilsPlugin.HasGyroscope())
		{
			_significantMotionPlugin.Init( OnSignificantMotion);
			_significantMotionPlugin.SetDebug(0);
			_significantMotionPlugin.RegisterSensorListener();
		}else
		{
			Debug.LogWarning("Gyroscope on current device is not available!");	
		}
	}
	
	private void OnApplicationPause(bool val){
		if(val){
			if(_significantMotionPlugin!=null){
				_significantMotionPlugin.RemoveSensorListener();
			}
		}else{
			if(_significantMotionPlugin!=null){
				_significantMotionPlugin.RegisterSensorListener();
			}
		}
	}

	public void ClickReset()
	{
		statusText.text = $"waiting for significant motion..";
	}
	#endregion

	#region Events
	private void OnSignificantMotion(){
		if (statusText!=null)
		{
			statusText.text = $"detect significant motion!";
			// register again because it will auto remove and disable after it trigers
			if(_significantMotionPlugin!=null){
				_significantMotionPlugin.RegisterSensorListener();
			}
		}
	}
	#endregion
}

