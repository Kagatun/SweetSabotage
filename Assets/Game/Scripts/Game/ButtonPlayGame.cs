using InterfaceUI;
using ManagementUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ButtonPlayGame : ButtonHandler
    {
        [SerializeField] private Image _panelOpen;
        [SerializeField] private Image _panelClose;
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private AudioSource _music;

        protected override void OnEnable()
        {
            base.OnEnable();
            _inputDetector.TabPressed += OnButtonClick;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputDetector.TabPressed -= OnButtonClick;
        }

        protected override void OnButtonClick()
        {
            _music.UnPause();
            Time.timeScale = 1.0f;
            _inputDetector.SetStatusNotPressed();
            _panelOpen.gameObject.SetActive(true);
            _panelClose.gameObject.SetActive(false);
        }
    }
}
