using InterfaceUI;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Shop
{
    public class PurchaseHandler : ButtonHandler
    {
        private const string ProductIDNoADS = "NoADS";
        private const string ProductIDGold500 = "Gold500";
        private const string ProductIDGold1500 = "Gold1500";
        private const string ProductIDGold2500 = "Gold2500";
        private const string ProductIDGold5000 = "Gold5000";
        private const string ProductIDGold15000 = "Gold15000";

        [SerializeField] private List<GameObject> _objectsToDisable;
        [SerializeField] private ScoreView _scoreView;

        private void Start()
        {
            if (YandexGame.SDKEnabled)
                OnDisableObjects();
        }

        protected override void OnEnableAction()
        {
            YandexGame.GetDataEvent += OnDisableObjects;
            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccess;
            YandexGame.RewardVideoEvent += OnSetReward;
            YandexGame.ConsumePurchases();
        }

        protected override void OnDisableAction()
        {
            YandexGame.GetDataEvent -= OnDisableObjects;
            YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccess;
            YandexGame.RewardVideoEvent -= OnSetReward;
        }

        protected override void OnButtonClick()
        {
            YandexGame.RewVideoShow(0);
        }

        private void OnDisableObjects()
        {
            if (YandexGame.savesData.IsBuy)
            {
                foreach (var obj in _objectsToDisable)
                    obj.SetActive(false);

                YandexGame.StickyAdActivity(false);
            }
            else if (YandexGame.savesData.IsBuy == false && YandexGame.savesData.IsDesktop == false)
            {
                YandexGame.StickyAdActivity(false);
            }
            else
            {
                foreach (var obj in _objectsToDisable)
                    obj.SetActive(true);

                YandexGame.StickyAdActivity(true);
            }
        }

        private void OnSetReward(int id)
        {
            YandexGame.savesData.Gold += 150;
            _scoreView.DisplayInvoice();
            YandexGame.SaveProgress();
        }

        private void OnPurchaseSuccess(string id)
        {
            switch (id)
            {
                case ProductIDNoADS:
                    YandexGame.savesData.IsBuy = true;

                    foreach (var obj in _objectsToDisable)
                        obj.SetActive(false);

                    break;

                case ProductIDGold500:
                    YandexGame.savesData.Gold += 500;
                    break;

                case ProductIDGold1500:
                    YandexGame.savesData.Gold += 1500;
                    break;

                case ProductIDGold2500:
                    YandexGame.savesData.Gold += 2500;
                    break;

                case ProductIDGold5000:
                    YandexGame.savesData.Gold += 5000;
                    break;

                case ProductIDGold15000:
                    YandexGame.savesData.Gold += 15000;
                    break;
            }

            _scoreView.DisplayInvoice();
            YandexGame.SaveProgress();
        }
    }
}