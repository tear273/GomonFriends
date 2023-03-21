using System;
using UnityEngine;

public class ShakePlugin : MonoBehaviour {
	
	private static ShakePlugin instance;
	private static GameObject container;
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static ShakePlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="ShakePlugin";
			instance = container.AddComponent( typeof(ShakePlugin) ) as ShakePlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.shake.ShakePlugin");
		}
		#endif
	}
	
	/// <summary>
	/// Sets the debug.
	/// 0 - false, 1 - true
	/// </summary>
	/// <param name="debug">Debug.</param>
	public void SetDebug(int debug){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("SetDebug",debug);
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}
	
	
	
	public void Init(Action <int,float>OnShake,Action<string> OnAccelerometer){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			IShakeCallback ishakeCallback = new IShakeCallback();
			ishakeCallback.OnShake = OnShake;
			ishakeCallback.OnAccelerometer = OnAccelerometer;
			jo.CallStatic("init",currentActivity,ishakeCallback);
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Sets the sensitivity of shake,
	/// lower sensitivity means more sensitive,higher the means less sensitive.
	/// preferred value is 1100
	/// </summary>
	/// <param name="sensitivity">Sensitivity.</param>
	public void SetSensitivity(int sensitivity){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setSensitivity",sensitivity);
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Sets the delay update in miliseconds,
	/// lower values means receiving more shake events as in frequently
	/// higher means often or less event
	/// preferred value 150ms = 0.15 seconds
	/// </summary>
	/// <param name="delayUpdate">Delay update.</param>
	public void SetDelayUpdate(int delayUpdate){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setDelayUpdate",delayUpdate);
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}

	public void RegisterSensorListener(SensorDelay sensorDelay){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("registerSensorListener",(int)sensorDelay);
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}

	public void RemoveSensorListener(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("removeSensorListener");
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Resets the shake count to zero.
	/// </summary>
	public void ResetShakeCount(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("resetShakeCount");
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}

	private void Message(string message){
		if(isDebug){
			Debug.LogWarning(message);
		}
	}
}