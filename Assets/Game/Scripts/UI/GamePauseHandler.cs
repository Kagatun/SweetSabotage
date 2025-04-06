using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GamePauseHandler : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _music;
    [SerializeField] private Image _imagePause;

    private void Start()
    {
        if (FocusObserver.HasFocus == false)
        {
            OnPauseGame();
        }
        else
        {
            OnUnPauseGame();
        }
    }

    private void OnEnable()
    {
        FocusObserver.ApplicationFocus += OnFocus;
        FocusObserver.ApplicationPause += OnPause;
        YandexGame.onVisibilityWindowGame += OnYandexVisibilityChanged;
    }

    private void OnDisable()
    {
        FocusObserver.ApplicationFocus -= OnFocus;
        FocusObserver.ApplicationPause -= OnPause;
        YandexGame.onVisibilityWindowGame -= OnYandexVisibilityChanged;
    }

    private void OnYandexVisibilityChanged(bool visible)
    {
        if (visible)
        {
            if (_imagePause == null || _imagePause.gameObject.activeSelf == false)
            {
                OnUnPauseGame();
            }
        }
        else
        {
            OnPauseGame();
        }
    }

    private void OnFocus(bool hasFocus)
    {
        if (FocusObserver.IsTransitioning)
        {
            if (hasFocus == false)
            {
                OnPauseGame();
            }
            else
            {
                OnUnPauseGame();
            }

            return;
        }

        // Если перехода нет
        if (hasFocus == false)
        {
            OnPauseGame();
        }
        else
        {
            if (_imagePause == null || _imagePause.gameObject.activeSelf == false)
            {
                OnUnPauseGame();
            }
        }
    }

    private void OnPause(bool pauseStatus)
    {
        if (FocusObserver.IsTransitioning)
        {
            if (pauseStatus)
            {
                OnPauseGame();
            }
            else
            {
                OnUnPauseGame();
            }

            return;
        }

        if (pauseStatus)
        {
            OnPauseGame();
        }
        else
        {
            if (_imagePause == null || !_imagePause.gameObject.activeSelf)
            {
                OnUnPauseGame();
            }
        }
    }

    private void OnPauseGame()
    {
        OnMute();
        Time.timeScale = 0;
    }

    private void OnUnPauseGame()
    {
        OnPlay();
        Time.timeScale = 1;
    }

    private void OnMute()
    {
        for (int i = 0; i < _music.Count; i++)
            _music[i].Pause();
    }

    private void OnPlay()
    {
        for (int i = 0; i < _music.Count; i++)
            _music[i].UnPause();
    }
}
