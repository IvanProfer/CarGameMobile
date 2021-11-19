using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Services.Ads.UnityAds
{
    public class UnityAdsService : MonoBehaviour, IUnityAdsListener, IUnityAdsInitializationListener
    {
        [SerializeField] private string _gameId = "";
        [SerializeField] private string _interstitialAds = "";
        [SerializeField] private string _rewardedAds = "";
        [SerializeField] private string _bannerAds = "";

        private Action _callback;
        public bool IsInitialized { get; private set; }

        public event Action Initializaed;

        private void Start()
        {
            Advertisement.Initialize(_gameId, true, false, this);
        }

        public void ShowInterstitial()
        {
            Advertisement.Show(_interstitialAds);
        }

        public void ShowRewarded()
        {
            Advertisement.Show(_rewardedAds);
        }
        public void OnUnityAdsDidError(string message)
        {
            
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if(showResult == ShowResult.Finished)
            {
                _callback?.Invoke();
            }
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            
        }

        public void OnUnityAdsReady(string placementId)
        {
            
        }

        public void OnInitializationComplete()
        {
            IsInitialized = true;
            Initializaed?.Invoke();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            
        }
    }
}