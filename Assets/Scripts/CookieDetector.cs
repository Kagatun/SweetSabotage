using System.Collections.Generic;
using UnityEngine;

public class CookieDetector : MonoBehaviour
{
    private List<TeleporterFigure> _figures = new List<TeleporterFigure>();
    private List<Cookie> _cookies = new List<Cookie>();
    private TeleporterFigure _currentFigure;

    private void Update()
    {
        if (_figures.Count > 0 && _cookies.Count > 0)
            DetermineShapeForCookie();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Cookie cookie))
        {
            cookie.EnableMover();
            _cookies.Add(cookie);
        }

        if (other.gameObject.TryGetComponent(out TeleporterFigure figure))
        {
            _currentFigure = figure;
            figure.Used += OnAddFigure;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out TeleporterFigure figure))
        {
            if (_currentFigure == figure)
                _currentFigure = null;

            figure.Used -= OnAddFigure;
        }
    }

    private void OnAddFigure()
    {
        _figures.Add(_currentFigure);
        _currentFigure.Used -= OnAddFigure;
        _currentFigure = null;
    }

    private void DetermineShapeForCookie()
    {
        List<TeleporterFigure> figuresToRemove = new List<TeleporterFigure>();
        List<Cookie> cookiesToRemove = new List<Cookie>();

        foreach (var figure in _figures)
        {
            var storage = figure.CookieStorage;

            if (storage.ISHoldersFilled())
            {
                figuresToRemove.Add(figure);
                continue;
            }

            foreach (var cookie in _cookies)
            {
                if (cookiesToRemove.Contains(cookie))
                    continue;

                if (cookie.Color == figure.Color)
                {
                    var freePoint = storage.GetFreePoint();

                    if (freePoint != null)
                    {
                        freePoint.Reserve();
                        cookie.SetTarget(freePoint);
                        cookiesToRemove.Add(cookie);
                    }
                }
            }
        }

        foreach (var cookie in cookiesToRemove)
            _cookies.Remove(cookie);

        foreach (var figure in figuresToRemove)
        {
            figure.Used -= OnAddFigure;
            figure.Remove();
            _figures.Remove(figure);
        }
    }
}