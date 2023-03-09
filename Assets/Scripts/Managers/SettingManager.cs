using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Singletone<SettingManager>
{
    private void Start()
    {
        DontDestroyOnLoad(this);
        Initalize();
    }

    void Initalize()
    {

    }
}
