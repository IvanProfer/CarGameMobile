using System;
using System.Collections;
using System.Collections.Generic;
using Features.Rewards.Slot;
using Features.Rewards.Currency;
using Tool;
using Profile;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.Rewards
{
    internal class RewardController : BaseController
    {
        private readonly ResourcePath _resourcePath = new ResourcePath("Prefabs/Rewards/DailyRewardsWindow Variant");
        private readonly CurrencyController _currencyController;
        private readonly ProfilePlayer _profilePlayer;
        private readonly RewardView _view;

        private List<ContainerSlotRewardView> _slots;
        private Coroutine _coroutine;

        private bool _isGetReward;


        public RewardController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _view = LoadView(placeForUi);
            _profilePlayer = profilePlayer;
            _currencyController = CreateCurrencyController(placeForUi);

            InitView();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            DeinitView();
        }


        private RewardView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_resourcePath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<RewardView>();
        }

        private CurrencyController CreateCurrencyController(Transform placeForUi)
        {
            var currencyController = new CurrencyController(placeForUi);
            AddController(currencyController);

            return currencyController;
        }


        private void InitView()
        {
            InitSlots();
            RefreshUi();
            StartRewardsUpdating();
            SubscribeButtons();
        }

        private void DeinitView()
        {
            DeinitSlots();
            StopRewardsUpdating();
            UnsubscribeButtons();
        }


        private void InitSlots()
        {
            _slots = new List<ContainerSlotRewardView>();

            for (int i = 0; i < _view.Rewards.Count; i++)
            {
                ContainerSlotRewardView instanceSlot = CreateSlotRewardView();
                _slots.Add(instanceSlot);
            }
        }

        private ContainerSlotRewardView CreateSlotRewardView() =>
            Object.Instantiate(_view.ContainerSlotRewardPrefab, _view.MountRootSlotsReward, false);

        private void DeinitSlots()
        {
            foreach (ContainerSlotRewardView slot in _slots)
                Object.Destroy(slot.gameObject);

            _slots.Clear();
        }


        private void StartRewardsUpdating() =>
            _coroutine = _view.StartCoroutine(RewardsStateUpdater());

        private void StopRewardsUpdating()
        {
            if (_coroutine == null)
                return;

            _view.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator RewardsStateUpdater()
        {
            WaitForSeconds waitForSecond = new WaitForSeconds(1);

            while (true)
            {
                RefreshRewardsState();
                RefreshUi();
                yield return waitForSecond;
            }
        }


        private void RefreshRewardsState()
        {
            bool gotRewardEarlier = _view.TimeGetReward.HasValue;
            if (!gotRewardEarlier)
            {
                _isGetReward = true;
                return;
            }

            TimeSpan timeFromLastRewardGetting = DateTime.UtcNow - _view.TimeGetReward.Value;
            bool isDeadlineElapsed = timeFromLastRewardGetting.Seconds >= _view.TimeDeadline;
            bool isTimeToGetNewReward = timeFromLastRewardGetting.Seconds >= _view.TimeCooldown;

            if (isDeadlineElapsed)
                ResetRewardsState();

            _isGetReward = isTimeToGetNewReward;
        }

        private void ResetRewardsState()
        {
            _view.TimeGetReward = null;
            _view.CurrentSlotInActive = 0;
        }


        private void RefreshUi()
        {
            _view.GetRewardButton.interactable = _isGetReward;
            _view.TimerNewReward.text = GetTimerNewRewardText();
            RefreshSlots();
        }

        private string GetTimerNewRewardText()
        {
            if (_isGetReward)
                return "The reward is ready to be received!";

            if (_view.TimeGetReward.HasValue)
            {
                DateTime nextClaimTime = _view.TimeGetReward.Value.AddSeconds(_view.TimeCooldown);
                TimeSpan currentClaimCooldown = nextClaimTime - DateTime.UtcNow;
                string timeGetReward = $"{currentClaimCooldown.Days:D2}:{currentClaimCooldown.Hours:D2}:" +
                                       $"{currentClaimCooldown.Minutes:D2}:{currentClaimCooldown.Seconds:D2}";

                return $"Time to get the next reward: {timeGetReward}";
            }

            return string.Empty;
        }

        private void RefreshSlots()
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                Reward reward = _view.Rewards[i];
                int countCooldownPeriods = i + 1;
                bool isSelect = i == _view.CurrentSlotInActive;

                _slots[i].SetData(reward, countCooldownPeriods, isSelect);
            }
        }


        private void SubscribeButtons()
        {
            _view.GetRewardButton.onClick.AddListener(ClaimReward);
            _view.ResetButton.onClick.AddListener(ResetTimer);
            _view.CloseButton.onClick.AddListener(Close);
        }

        private void UnsubscribeButtons()
        {
            _view.GetRewardButton.onClick.RemoveListener(ClaimReward);
            _view.ResetButton.onClick.RemoveListener(ResetTimer);
            _view.CloseButton.onClick.RemoveListener(Close);
        }

        private void ClaimReward()
        {
            if (!_isGetReward)
                return;

            Reward reward = _view.Rewards[_view.CurrentSlotInActive];

            switch (reward.RewardType)
            {
                case RewardType.Wood:
                    _currencyController.AddWood(reward.CountCurrency);
                    break;
                case RewardType.Diamond:
                    _currencyController.AddDiamond(reward.CountCurrency);
                    break;
            }

            _view.TimeGetReward = DateTime.UtcNow;
            _view.CurrentSlotInActive++;

            RefreshRewardsState();
        }

        private void ResetTimer()
        {
            PlayerPrefs.DeleteAll();
            _currencyController.RefreshView();
        }

        private void Close()
        {
            _profilePlayer.CurrentState.Value = GameState.Start;
        }
    }
}
