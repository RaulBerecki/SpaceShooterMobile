using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour , IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdId;
    public GameManagerScript gameManagerScript;
    public UI_ManagerController uiManagerController;

    private string adUnitId;

    private void Start()
    {
        
    }
    private void Awake()
    {
#if UNITY_IOS
adUnitId = iosAdId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif
    }
    public void LoadRewardedAd()
    {
        Advertisement.Load(adUnitId, this);
    }
    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAd();
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        gameManagerScript.AdCompleted = true;
        uiManagerController.gameOverPanel.SetActive(false);
        uiManagerController.mainMenu.SetActive(true);
    }
}
