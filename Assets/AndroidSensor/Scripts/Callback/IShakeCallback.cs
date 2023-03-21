using System;
using UnityEngine;

public class IShakeCallback :  AndroidJavaProxy {	

	public Action <int,float>OnShake;
	public Action <string>OnAccelerometer;
	
	public IShakeCallback() : base("com.gigadrillgames.androidplugin.shake.IShakeCallback") {}

	
	void onShake(int count,float speed){
		OnShake(count,speed);
	}
	
	void onAccelerometer(string sensorData){
		OnAccelerometer(sensorData);
	}
}
