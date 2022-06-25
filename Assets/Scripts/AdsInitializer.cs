using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    private string _gameId = "4813946";    
    private bool _testMode = true;
    BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    private void OnEnable()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
        Advertisement.Initialize(_gameId, _testMode, this);
        Advertisement.Banner.SetPosition(_bannerPosition);
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_gameId, options);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    private void OnDisable()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    private void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        Advertisement.Banner.Show(_gameId);
    }
}
