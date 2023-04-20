using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string gameId = "5250572";

    public string adUnitId = "Free_FriendshipStar";
    Action func;
    // Start is called before the first frame update

    private void Awake()
    {
        Advertisement.Initialize(gameId);
    }
    void Start()
    {
        this.LoadAd();
    }

    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + adUnitId);
        Advertisement.Load(adUnitId, this);
    }
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.LogFormat("OnUnityAdsAdLoaded: {0}", placementId);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogFormat("OnUnityAdsFailedToLoad: {0}, {1}, {2}", placementId, error, message);
    }


    // Implement a method to execute when the user clicks the button.
    public void ShowAd(Action func)
    {
        this.func = func;
        // Then show the ad:
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogFormat("OnUnityAdsShowFailure: {0}, {1}, {2}", placementId, error, message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        GameManager.Instance.Block_Panel.gameObject.SetActive(true);
        NGUITools.BringForward(GameManager.Instance.Block_Panel.gameObject);
        Debug.LogFormat("OnUnityAdsShowStart: {0}", placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.LogFormat("OnUnityAdsShowClick: {0}", placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.LogFormat("OnUnityAdsShowComplete: {0}, {1}", placementId, showCompletionState.ToString());

        if (adUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            func();
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            Debug.Log("보상을 받았습니다.");

            // Load another ad:
            GameManager.Instance.Block_Panel.gameObject.SetActive(false);
            Advertisement.Load(adUnitId, this);
        }
    }
}