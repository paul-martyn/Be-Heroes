using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
    public class AdsService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidGameId = "5561122";
        private const string IOSGameId = "5561123";

        private const string AndroidRewardedAdUnit = "Rewarded_Android";
        private const string IOSRewardedAdUnit = "Rewarded_iOS";

        private string _gameId;
        private string _placementId;
        private Action _onVideoFinished;

        public event Action RewardedVideoReady;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _placementId = AndroidRewardedAdUnit;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _placementId = IOSRewardedAdUnit;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = AndroidGameId;
                    _placementId = AndroidRewardedAdUnit;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }

            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(_placementId);
            _onVideoFinished = onVideoFinished;
        }

        public void IsRewardedVideoReady() => 
            Advertisement.Load(_placementId, this);

        #region IUnityAdsInitializationListener

        public void OnInitializationComplete() =>
            Debug.Log($"AdsService InitializationComplete");

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) =>
            Debug.Log($"AdsService OnInitializationFailed");

        #endregion

        #region IUnityAdsLoadListener

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"OnUnityAdsAdLoaded {placementId}");

            if (placementId == _placementId)
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.Log($"OnUnityAdsFailedToLoad {placementId}; {error}; {message}.");

        #endregion

        #region IUnityAdsShowListener

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"OnUnityAdsShowFailure {placementId}; {error}; {message}.");

        public void OnUnityAdsShowStart(string placementId) =>
            Debug.Log($"OnUnityAdsShowStart {placementId}");

        public void OnUnityAdsShowClick(string placementId) =>
            Debug.Log($"OnUnityAdsShowClick {placementId}");

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log($"OnUnityAdsShowComplete {placementId}; {showCompletionState}.");
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.COMPLETED:
                    _onVideoFinished?.Invoke();
                    break;
            }
            _onVideoFinished = null;
        }

        #endregion
    }
}