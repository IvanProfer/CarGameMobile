using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Services.Ads.UnityAds
{
    internal abstract class UnityAdsPlayer : IAdsPlayer, IUnityAdsListener
    {
        public event Action Started;
        public event Action Finished;
        public event Action Failed;
        public event Action Skipped;
        public event Action BecomeReady;

        protected readonly string _id;


        protected UnityAdsPlayer(string id) =>
            _id = id;


        public void Play()
        {
            Load();
            OnPlaying();
            Load();

            Log("Play");
        }

        protected abstract void OnPlaying();
        protected abstract void Load();


        public void OnUnityAdsReady(string placementId)
        {
            if (IsIdMy(placementId) == false)
                return;

            Log("Ready");
            BecomeReady?.Invoke();
        }

        public void OnUnityAdsDidError(string message)
        {
            Error($"Error: {message}");
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            if (IsIdMy(placementId) == false)
                return;

            Log("Started");
            Started?.Invoke();
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (IsIdMy(placementId) == false)
                return;

            switch (showResult)
            {
                case ShowResult.Finished:
                    Log("Finished");
                    Finished?.Invoke();
                    break;

                case ShowResult.Failed:
                    Error("Failed");
                    Failed?.Invoke();
                    break;

                case ShowResult.Skipped:
                    Log("Skipped");
                    Skipped?.Invoke();
                    break;
            }
        }

        private bool IsIdMy(string id) => _id == id;

        private void Log(string message) => Debug.Log(WrapMessage(message));
        private void Error(string message) => Debug.LogError(WrapMessage(message));
        private string WrapMessage(string message) => $"[{GetType().Name}] {message}";
    }
}
