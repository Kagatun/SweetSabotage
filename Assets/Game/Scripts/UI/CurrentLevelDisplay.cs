using TMPro;
using UnityEngine;
using YG;

namespace InterfaceUI
{
    public class CurrentLevelDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private int _level;

        private void Start()
        {
            _level = YandexGame.savesData.LevelNumber + 1;
            _text.text = $"{_level}";
        }
    }
}
