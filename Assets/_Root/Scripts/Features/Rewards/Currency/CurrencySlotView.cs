using TMPro;
using UnityEngine;

namespace Features.Rewards.Currency
{
    internal class CurrencySlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _count;


        public void SetData(int count) =>
            _count.text = count.ToString();
    }
}
