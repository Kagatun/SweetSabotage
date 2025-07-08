using UnityEngine;
using YG;

namespace InterfaceUI
{
    public class LevelActivator : ButtonHandler
    {
        [SerializeField] private AdapterBetweenScenes _adapterBetweenScenes;
        [SerializeField] private int _indexLevel;

        public int IndexLevel => _indexLevel;

        protected override void OnButtonClick()
        {
            Time.timeScale = 1;
            
            if (_indexLevel == 0)
            {
                _adapterBetweenScenes.LoadExitScene(0);
            }
            else
            {
                YandexGame.savesData.LevelNumber = _indexLevel - 1;
                YandexGame.SaveProgress();
                _adapterBetweenScenes.LoadExitScene(1);
            }
        }
    }
}