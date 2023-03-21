using System;
using UnityEngine;

public class ISignificantMotionCallback :  AndroidJavaProxy {
    public Action OnSignificantMotion;
    public ISignificantMotionCallback() : base("com.gigadrillgames.androidplugin.significantmotion.ISignificantMotionCallback") {}
    void onSignificantMotion(){
        OnSignificantMotion();
    }
}


