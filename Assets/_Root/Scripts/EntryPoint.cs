using System.Collections.Generic;
using Profile;
using UnityEngine;
using UnityEngine.Analytics;
using Services.Analytics;
using Services.Ads.UnityAds;
using Services.IAP;



internal class EntryPoint : MonoBehaviour
{
    private const float SpeedCar = 15f;
    private const float BoatSpeed = 20f;
    private const GameState InitialState = GameState.Start;

    [SerializeField] private Transform _placeForUi;
    [SerializeField] private UnityAdsService _adsService;
    [SerializeField] private IAPService _iapService;

    private MainController _mainController;
    [SerializeField] private AnalyticsManager _analytics;


    private void Awake()
    {
        var profilePlayer = new ProfilePlayer(SpeedCar, InitialState);
        _mainController = new MainController(_placeForUi, profilePlayer);

        

        _iapService.Initialized.AddListener(() => _iapService.Buy("1"));

        if (_iapService._isInitialized)
        {
            _iapService.Buy("1");
            Analytics.Transaction("1", 10, "RUB", null, null);
            Debug.Log("Analytics+");
        }
        else
        {
            _iapService.Initialized.AddListener(
                () => _iapService.Buy("1"));

        }
    }

    

    

    private void OnDestroy()
    {
        _mainController.Dispose();
    }
}
