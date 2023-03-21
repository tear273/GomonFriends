using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * StaticManager
 *
 * 1. 언어 가져오기
 * 2. 게임 실행
 */

public class StaticManager : Singletone<StaticManager>
{


    public static UIManager UI { get; private set; }

    public static BackendManager Backend { get; private set; }

    public static SoundManager Sound { get; private set; }

    //public static ADManager AD { get; private set; }

   // public static IAPManager IAP { get; private set; }

    //public static SaveManager Save { get; private set; }

    public static ChartManager Chart { get; private set; }

    //public static FirebaseManager Firebase { get; private set; }

    void Awake()
    {
        if (transform.parent != null && transform.root != null)
            DontDestroyOnLoad(this.transform.root.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);

        Initialize();
    }

    public void Initialize()
    {
        Backend = GetComponentInChildren<BackendManager>();
        UI = GetComponentInChildren<UIManager>();
        Sound = GetComponentInChildren<SoundManager>();
       // AD = GetComponentInChildren<ADManager>();
      //  IAP = GetComponentInChildren<IAPManager>();
       // Save = GetComponentInChildren<SaveManager>();
        Chart = GetComponentInChildren<ChartManager>();
       // Firebase = GetComponentInChildren<FirebaseManager>();

        UI.Initialize();
        Backend.Initialize();
      //  AD.Initialize();
    }

   /* public void ChangeScene(string sceneName, FadeUI.FadeType fadeType = FadeUI.FadeType.ChangeToBlack, float duration = 1)
    {
        UI.FadeUI.FadeStart(fadeType, () => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName), duration);
    }*/
}
