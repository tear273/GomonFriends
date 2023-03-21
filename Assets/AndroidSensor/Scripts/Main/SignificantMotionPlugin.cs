using System;
using UnityEngine;

public class SignificantMotionPlugin : MonoBehaviour {
	
	private static SignificantMotionPlugin instance;
	private static GameObject container;
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static SignificantMotionPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="SignificantMotionPlugin";
			instance = container.AddComponent( typeof(SignificantMotionPlugin) ) as SignificantMotionPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.significantmotion.SignificantMotionPlugin");
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
	
	public void Init(Action OnSignificantMotion){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			ISignificantMotionCallback iSignificantMotionCallback = new ISignificantMotionCallback();
			iSignificantMotionCallback.OnSignificantMotion = OnSignificantMotion;
			jo.CallStatic("init",currentActivity,iSignificantMotionCallback);
		}else{
			Message("warning: must run in actual android device");
		}
		#endif
	}

	public void RegisterSensorListener(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("registerSensorListener");
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

	private void Message(string message){
		if(isDebug){
			Debug.LogWarning(message);
		}
	}
}