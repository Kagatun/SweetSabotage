using InterfaceUI;
using ManagementUtilities;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Game
{
    public class ButtonStopPlayGame : ButtonHandler
    {
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private Image _panelOpen;
        [SerializeField] private Image _panelClose;
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _soundMenu;

        protected override void OnEnable()
        {
            base.OnEnable();
            _inputDetector.EscPressed += OnButtonClick;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputDetector.EscPressed -= OnButtonClick;
        }

        protected override void OnButtonClick()
        {
            if (YandexGame.savesData.IsBuy == false)
                YandexGame.FullscreenShow();

            _panelOpen.gameObject.SetActive(true);
            _panelClose.gameObject.SetActive(false);
            _inputDetector.SetStatusPressed();
            Time.timeScale = 0;
            _soundMenu.Play();
            _music.Pause();
        }
    }
}
