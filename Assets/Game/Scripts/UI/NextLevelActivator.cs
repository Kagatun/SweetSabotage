using UnityEngine;
using UnityEngine.SceneManagement;

namespace InterfaceUI
{
    public class NextLevelActivator : ButtonHandler
    {
        [SerializeField] private AdapterBetweenScenes _adapterBetweenScenes;

        private int _nextLevelIndex;

        private void Start()
        {
            int indexAdd = 1;
            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            _nextLevelIndex = levelIndex + indexAdd;
        }

        protected override void OnButtonClick()
        {
            Time.timeScale = 1;
            _adapterBetweenScenes.LoadExitScene(_nextLevelIndex);
        }
    }
}