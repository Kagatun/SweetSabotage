using System;
using UnityEngine;

public class RemovalZone : MonoBehaviour
{
    private int _colorMatchCounter = 0;
    private Renderer _render;
    private Color _currentColor;
    private TeleporterFigure _currentFigure;

    public event Action<int> PenaltyTime;

    private void Awake()
    {
        _render = GetComponent<Renderer>();
        _currentColor = _render.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out TeleporterFigure figure))
        {
            figure.Used += OnFigureUsed;
            figure.SetStatusRemove();
            _currentFigure = figure;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out TeleporterFigure figure))
        {
            figure.Used -= OnFigureUsed;
            figure.ResetStatusRemove();
            _currentFigure = null;
        }
    }

    private void OnFigureUsed()
    {
        if (_currentFigure == null) 
            return;

        if (_currentColor == _currentFigure.Color)
        {
            if (_colorMatchCounter == 0)
            {
                PenaltyTime?.Invoke(0);
                _colorMatchCounter++;
            }
            else if (_colorMatchCounter == 1)
            {
                int minPenalty = 1;
                int divider = 2;
                int penaltySecondMin = Mathf.Max(minPenalty, _currentFigure.CookieStorage.CookieHoldersCount / divider);

                PenaltyTime?.Invoke(penaltySecondMin);
                _colorMatchCounter++;
            }
        }
        else
        {
            PenaltyTime?.Invoke(_currentFigure.CookieStorage.CookieHoldersCount);
            _currentColor = _currentFigure.Color;
            _colorMatchCounter = 1;
        }

        _currentFigure.SetStatusRemove();
        _render.material.color = _currentColor;
        _currentFigure.Used -= OnFigureUsed;
        _currentFigure = null;
    }
}