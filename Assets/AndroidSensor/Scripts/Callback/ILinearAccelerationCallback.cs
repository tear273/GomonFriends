using System;
using UnityEngine;

public class ILinearAccelerationCallback :  AndroidJavaProxy {
    public Action <string>OnLinearAcceleration;
    public ILinearAccelerationCallback() : base("com.gigadrillgames.androidplugin.linearacceleration.ILinearAccelerationCallback") {}
    void onLinearAcceleration(string sensorData){
        OnLinearAcceleration(sensorData);
    }
}

