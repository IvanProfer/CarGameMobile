using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private Button _buttonBack;


        public void Back(UnityAction Back) =>
            _buttonBack.onClick.AddListener(Back);

        public void OnDestroy() =>
            _buttonBack.onClick.RemoveAllListeners();
    }
}