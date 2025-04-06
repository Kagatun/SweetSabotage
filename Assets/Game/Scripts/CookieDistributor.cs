using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class CookieDistributor : MonoBehaviour
{
    [SerializeField] private Timeline _timeline;
    [SerializeField] private ScoreCounter _scoreCounter;

    private List<TeleporterFigure> _figures = new List<TeleporterFigure>();
    private List<TeleporterFigure> _figuresToRemove = new List<TeleporterFigure>();
    private List<TeleporterFigure> _figuresInRemoval = new List<TeleporterFigure>();

    private List<Cookie> _cookies = new List<Cookie>();
    private TeleporterFigure _currentFigure;

    public event Action <int> Counted;

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

            if (_cookies.Contains(cookie) == false)
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
        _currentFigure.Removed += RemoveFigure;
        _currentFigure.Used -= OnAddFigure;
        _currentFigure = null;
    }

    private void RemoveFigure(TeleporterFigure figure)
    {
        figure.Removed -= RemoveFigure;
        _figures.Remove(figure);
        _figuresToRemove.Remove(figure);
        _figuresInRemoval.Remove(figure);
        _scoreCounter.AddScore(figure.CookieStorage.CookieHoldersCount);
        RemoveTime(figure);
        Counted?.Invoke(figure.CookieStorage.CookieHoldersCount);
    }

    private void RemoveTime(TeleporterFigure figure)
    {
        int minChance = 0;
        int maxChance = 100;
        int multiplier = 7;
        int currentChance = YandexGame.savesData.ChanceRemoveTime * multiplier;
        int chance = UnityEngine.Random.Range(minChance, maxChance);

        if (chance < currentChance)
        {
            int extraPoint = 1;
            _timeline.RemoveTime(figure.CookieStorage.CookieHoldersCount + extraPoint);
        }
        else
        {
            _timeline.RemoveTime(figure.CookieStorage.CookieHoldersCount);
        }
    }

    private void DetermineShapeForCookie()
    {
        List<Cookie> cookiesToRemove = new List<Cookie>();

        CheckFiguresInRemoval();
        ProcessFigures(cookiesToRemove);
        RemoveDistributedCookies(cookiesToRemove);
        StartRemovalCoroutines();
    }

    private void CheckFiguresInRemoval()
    {
        for (int i = _figuresToRemove.Count - 1; i >= 0; i--)
        {
            var figure = _figuresToRemove[i];
            var storage = figure.CookieStorage;

            if (storage.ISHoldersFilled() == false)
            {
                _figuresToRemove.RemoveAt(i);
            }
        }
    }

    private void ProcessFigures(List<Cookie> cookiesToRemove)
    {
        List<Cookie> cookies = new List<Cookie>(_cookies);

        for (int i = 0; i < _figures.Count; i++)
        {
            var figure = _figures[i];
            var storage = figure.CookieStorage;

            if (storage.ISHoldersFilled() && _figuresToRemove.Contains(figure) == false && _figuresInRemoval.Contains(figure) == false)
            {
                _figuresToRemove.Add(figure);
            }

            if (storage.ISHoldersFilled() == false && _figuresInRemoval.Contains(figure))
            {
                _figuresInRemoval.Remove(figure);
            }

            if (storage.ISHoldersFilled() == false)
            {
                DistributeCookiesForFigure(figure, cookies, cookiesToRemove);
            }
        }
    }

    private void DistributeCookiesForFigure(TeleporterFigure figure, List<Cookie> cookies, List<Cookie> cookiesToRemove)
    {
        foreach (var cookie in cookies)
        {
            if (cookiesToRemove.Contains(cookie))
                continue;

            if (cookie.Color == figure.Color)
            {
                var freePoint = figure.CookieStorage.GetFreePoint();

                if (freePoint != null)
                {
                    freePoint.Reserve();
                    cookie.SetTarget(freePoint);
                    cookiesToRemove.Add(cookie);
                }
            }
        }
    }

    private void RemoveDistributedCookies(List<Cookie> cookiesToRemove)
    {
        foreach (var cookie in cookiesToRemove)
            _cookies.Remove(cookie);
    }

    private void StartRemovalCoroutines()
    {
        for (int i = _figuresToRemove.Count - 1; i >= 0; i--)
        {
            var figure = _figuresToRemove[i];
            figure.StartCoroutineRemove();
            _figuresToRemove.RemoveAt(i);
            _figuresInRemoval.Add(figure);
        }
    }
}