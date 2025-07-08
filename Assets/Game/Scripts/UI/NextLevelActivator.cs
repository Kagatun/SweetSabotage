using UnityEngine;
using YG;

namespace InterfaceUI
{
    public class NextLevelActivator : ButtonHandler
    {
        [SerializeField] private AdapterBetweenScenes _adapterBetweenScenes;

        private void Start()
        {
            int maxLevel = 69;
            
            if(YandexGame.savesData.LevelNumber >= maxLevel)
                gameObject.SetActive(false);
        }

        protected override void OnButtonClick()
        {
            YandexGame.savesData.LevelNumber ++;
            YandexGame.SaveProgress();
            Time.timeScale = 1;
            _adapterBetweenScenes.LoadExitScene(1);
        }
    }
}