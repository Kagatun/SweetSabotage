using UnityEngine;
using UnityEngine.UI;

public class ButtonPlayGame : ButtonHandler
{
    [SerializeField] private Image _panelOpen;
    [SerializeField] private Image _panelClose;
    [SerializeField] private InputDetector _inputDetector;
    [SerializeField] private AudioSource _music;

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
        _music.UnPause();
        Time.timeScale = 1.0f;
        _inputDetector.SetStatusNotPressed();
        _panelOpen.gameObject.SetActive(true);
        _panelClose.gameObject.SetActive(false);
    }
}
