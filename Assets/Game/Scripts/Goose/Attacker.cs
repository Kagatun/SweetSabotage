using System;
using System.Collections;
using UnityEngine;

public class Attacker
{
    private AnimationsGoose _animations;
    private FigureDetector _figureDetector;
    private Mover _mover;
    private CookieHolder _targetForTurn;

    private float _attackDistance;
    private float _timeWait = 0.5f;
    private float _timeRemove = 0.4f;
    private WaitForSeconds _wait;
    private WaitForSeconds _waitRemove;
    private Coroutine _attackCoroutine;

    public event Action Hited;

    public Attacker(float attackDistance, AnimationsGoose animations, FigureDetector figureDetector, Mover mover)
    {
        _attackDistance = attackDistance;
        _animations = animations;
        _figureDetector = figureDetector;
        _mover = mover;

        _wait = new WaitForSeconds(_timeWait);
        _waitRemove = new WaitForSeconds(_timeRemove);
    }

    public TeleporterFigure CurrentFigure { get; private set; }

    public void Attack(Color color, Transform goose, Action onAttackComplete)
    {
        CookieHolder nearestCookieHolder = FindNearestCookieHolder(color);

        if (nearestCookieHolder != null)
        {
            _targetForTurn = nearestCookieHolder;
            _animations.TriggerAttack();
            _mover.GoToTarget(null);

            RotateTarget(goose);
            _attackCoroutine = _animations.StartCoroutine(PerformAttack(goose, onAttackComplete));
        }
        else
        {
            onAttackComplete?.Invoke();
        }
    }

    public void StopAttack()
    {
        if(_attackCoroutine != null && _animations != null)
            _animations.StopCoroutine(_attackCoroutine);
    }

    private IEnumerator PerformAttack(Transform goose, Action onAttackComplete)
    {
        yield return _wait;

        RemoveCookie();
        Hited?.Invoke();

        yield return _waitRemove;

        onAttackComplete?.Invoke();
    }

    private void RemoveCookie()
    {
        if (_targetForTurn != null)
            _targetForTurn.TakeDamage();
    }

    private void RotateTarget(Transform goose)
    {
        Vector3 direction = (_targetForTurn.transform.position - goose.position).normalized;
        goose.forward = direction;
    }

    private CookieHolder FindNearestCookieHolder(Color color)
    {
        CookieHolder nearestCookieHolder = null;
        float nearestDistanceSqr = _attackDistance;

        foreach (var figure in _figureDetector.Figures)
        {
            if (figure.Color != color)
                continue;

            foreach (var cookieHolder in figure.CookieStorage.GetCookieHolders())
            {
                if (!cookieHolder.HasCookies())
                    continue;

                float distanceSqr = (cookieHolder.transform.position - _figureDetector.transform.position).sqrMagnitude;

                if (distanceSqr <= nearestDistanceSqr)
                {
                    nearestDistanceSqr = distanceSqr;
                    nearestCookieHolder = cookieHolder;
                    CurrentFigure = figure;
                }
            }
        }

        return nearestCookieHolder;
    }
}