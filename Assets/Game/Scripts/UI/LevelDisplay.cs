using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace InterfaceUI
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private LevelActivator _levelActivator;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _imageBlock;

        private int _currentLevelIndex;

        private void Start()
        {
            if (YandexGame.SDKEnabled)
                OpenLevel();
        }

        private void OnEnable()
        {
            YandexGame.GetDataEvent += OpenLevel;
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= OpenLevel;
        }

        private void OpenLevel()
        {
            _currentLevelIndex = YandexGame.savesData.LevelIndex;

            if (_currentLevelIndex >= _levelActivator.IndexLevel)
                _imageBlock.gameObject.SetActive(false);

            _text.text = $"{_levelActivator.IndexLevel}";
        }
    }
}
