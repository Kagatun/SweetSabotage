using InterfaceUI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using YG;

namespace Game
{
    public class GoldCounter : ButtonHandler
    {
        [SerializeField] private CookieDistributor _cookieDistributor;
        [SerializeField] private TextMeshProUGUI _textGold;
        [SerializeField] private Image _panelGold;

        private int _gold;

        private void Start()
        {
            if (YandexGame.savesData.IsBuy)
                ActionButton.gameObject.SetActive(false);
        }

        public void ShowGold()
        {
            AddBonusGold();
            _panelGold.gameObject.SetActive(true);
            _textGold.text = $"{_gold}";
            YandexGame.savesData.Gold += _gold;
            YandexGame.SaveProgress();
        }
        
        protected override void OnEnableAction()
        {
            YandexGame.RewardVideoEvent += OnAddGold;
            _cookieDistributor.Counted += OnCounted;
        }

        protected override void OnDisableAction()
        {
            YandexGame.RewardVideoEvent -= OnAddGold;
            _cookieDistributor.Counted -= OnCounted;
        }
        
        protected override void OnButtonClick()
        {
            ActionButton.gameObject.SetActive(false);
            YandexGame.RewVideoShow(0);
        }

        private void AddBonusGold()
        {
            if (YandexGame.savesData.IsBuy)
            {
                int multiplier = 2;
                _gold *= multiplier;
            }

            int multiplierBonus = 7;
            int divider = 100;
            int bonus = YandexGame.savesData.ExtraCharge * multiplierBonus;
            int percentAdd = _gold * bonus / divider;
            _gold += percentAdd;

            int currentIndexLevel = SceneManager.GetActiveScene().buildIndex;
            _gold += currentIndexLevel;
        }

        private void OnCounted(int value)
        {
            _gold += value;
        }

        private void OnAddGold(int id)
        {
            int multiplier = 2;

            YandexGame.savesData.Gold += _gold;
            _gold *= multiplier;
            _textGold.text = $"{_gold}";
            YandexGame.SaveProgress();
        }
    }
}
