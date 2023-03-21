using AUP;
using UnityEngine;

public class SensorUtilsPlugin : MonoBehaviour
{
	
	private static SensorUtilsPlugin instance;
	private static GameObject container;
	private static AUPHolder aupHolder;
	private const string TAG = "[SensorUtilsPlugin]: ";
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif
	
	public bool isDebug = true;

	public static SensorUtilsPlugin GetInstance ()
	{
		if (instance == null) {
			container = new GameObject ();
			container.name = "SensorUtilsPlugin";
			instance = container.AddComponent (typeof(SensorUtilsPlugin)) as SensorUtilsPlugin;
			DontDestroyOnLoad (instance.gameObject);
			aupHolder = AUPHolder.GetInstance ();
			instance.gameObject.transform.SetParent (aupHolder.gameObject.transform);
		}
		
		return instance;
	}

	private void Awake ()
	{
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			jo = new AndroidJavaObject ("com.gigadrillgames.androidplugin.sensorutils.SensorUtilsPlugin");
		}
		#endif
	}
	
	public void Init(){
		
#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.CallStatic("init",currentActivity);
		}else{
			Message("warning: must run in actual android device");
		}
#endif
	}

	/// <summary>
	/// Sets the debug.
	/// 0 - false, 1 - true
	/// </summary>
	/// <param name="debug">Debug.</param>
	public void SetDebug (int debug)
	{
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			jo.CallStatic ("SetDebug", debug);
		} else {
			SensorUtils.Message (TAG, "warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Determines whether the current android device has a step detector
	/// </summary>
	/// <returns><c>true</c> if this current android device has a step detector feature; otherwise, <c>false</c>.</returns>
	public bool HasStepDetector ()
	{
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			return jo.CallStatic<bool> ("hasStepDetector");
		} else {
			SensorUtils.Message (TAG, "warning: must run in actual android device");
		}
		#endif
		
		return false;
	}

	public bool HasStepCounter ()
	{
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			return jo.CallStatic<bool> ("hasStepCounter");
		} else {
			SensorUtils.Message (TAG, "warning: must run in actual android device");
		}
		#endif

		return false;
	}
	
	public bool HasAccelerometer()
	{
#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
		{
			return jo.CallStatic<bool>("hasAccelerometer");
		}
		else
		{
			Message("warning: must run in actual android device");
		}
#endif

		return false;
	}
	
	public bool HasGyroscope()
	{
#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
		{
			return jo.CallStatic<bool>("hasGyroscope");
		}
		else
		{
			Message("warning: must run in actual android device");
		}
#endif

		return false;
	}
	
	private void Message(string message){
		if(isDebug){
			Debug.LogWarning(message);
		}
	}
}