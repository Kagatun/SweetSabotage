using InterfaceUI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Shop
{
    public abstract class ImprovementHandler : ButtonHandler
    {
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private List<Image> _starsLevels;
        [SerializeField] private Image _imageDescription;
        [SerializeField] private List<TextMeshProUGUI> _specifications;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private AudioSource _audioBuy;
        [SerializeField] private AudioSource _audioError;

        private List<int> _pricesTool = new List<int> { 500, 1500, 2500, 5000, 10000 };
        private int _indexSave;

        private void Start()
        {
            _indexSave = GetIndexBuy();

            for (int i = 0; i < _indexSave; i++)
                _starsLevels[i].gameObject.SetActive(true);

            if (_indexSave > 0)
                _imageDescription.gameObject.SetActive(false);

            _specifications[_indexSave].gameObject.SetActive(true);

            TurnOffButton();

            if (_indexSave != _starsLevels.Count)
                _priceText.text = _pricesTool[_indexSave].ToString();
        }

        protected override void OnButtonClick()
        {
            if (YandexGame.savesData.Gold >= _pricesTool[_indexSave])
            {
                YandexGame.savesData.Gold -= _pricesTool[_indexSave];

                RenderBuy();
                _audioBuy.Play();
                _scoreView.DisplayInvoice();
            }
            else
            {
                _audioError.Play();
            }

            TurnOffButton();
            YandexGame.SaveProgress();
        }

        protected abstract int GetIndexBuy();

        protected virtual void UpdateIndex()
        {
            _indexSave++;
        }

        private void TurnOffButton()
        {
            if (_indexSave == _starsLevels.Count)
            {
                ActionButton.gameObject.SetActive(false);
                _priceText.gameObject.SetActive(false);
            }
        }

        private void RenderBuy()
        {
            _priceText.text = _pricesTool[_indexSave].ToString();
            _specifications[_indexSave].gameObject.SetActive(false);
            _imageDescription.gameObject.SetActive(false);
            _starsLevels[_indexSave].gameObject.SetActive(true);

            UpdateIndex();

            _specifications[_indexSave].gameObject.SetActive(true);

            if (_indexSave < _starsLevels.Count)
                _priceText.text = _pricesTool[_indexSave].ToString();
        }
    }
}
