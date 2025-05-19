using UnityEngine;

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
            _adapterBetweenScenes.LoadExitScene(_indexLevel);
        }
    }
}