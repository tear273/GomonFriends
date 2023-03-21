using System;
using UnityEngine;

public class IRotationVectorCallback :  AndroidJavaProxy {
    public Action <string>OnRotationVector;
    public IRotationVectorCallback() : base("com.gigadrillgames.androidplugin.rotationvector.IRotationVectorCallback") {}
    void onRotationVector(string sensorData){
        OnRotationVector(sensorData);
    }
}


