using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Button _buttonAds;


        public void Init(UnityAction startGame, UnityAction Ads)
        {
            _buttonStart.onClick.AddListener(startGame);
            _buttonAds.onClick.AddListener(Ads);
        }

        public void OnDestroy() =>
            _buttonStart.onClick.RemoveAllListeners();


    }
}
