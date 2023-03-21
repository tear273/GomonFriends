using System;
using UnityEngine;

public class IGravityCallback :  AndroidJavaProxy {
    public Action <string>OnGravity;
    public IGravityCallback() : base("com.gigadrillgames.androidplugin.gravity.IGravityCallback") {}
    void onGravity(string sensorData){
        OnGravity(sensorData);
    }
}

