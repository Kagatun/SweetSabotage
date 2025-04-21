using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InterfaceUI
{
    public class CurrentLevelDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private int _level;

        private void Start()
        {
            _level = SceneManager.GetActiveScene().buildIndex;
            _text.text = $"{_level}";
        }
    }
}
