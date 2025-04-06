using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private int _speed = 1;
    [SerializeField] private float _minDistanceToTargetSqr = 0.01f;
    [SerializeField] private float _flightAltitude = 0;

    private Transform _targetTransform;
    private int _baseSpeed;
    private Coroutine _speedBoostCoroutine;

    public event Action TargetReached;
    public event Action Slowed;

    private void Awake()
    {
        _baseSpeed = _speed;
    }

    private void Update()
    {
        if (_targetTransform != null)
            GoToTarget(_targetTransform);
    }

    public void GoToTarget(Transform positionTarget)
    {
        if (positionTarget == null)
        {
            _targetTransform = null;
            return;
        }

        _targetTransform = positionTarget;

        Vector3 direction = (_targetTransform.position - transform.position);

        if (direction.sqrMagnitude > _minDistanceToTargetSqr)
        {
            direction.Normalize();
            transform.forward = direction;

            Vector3 newPosition = transform.position + transform.forward * _speed * Time.deltaTime;
            newPosition.y = _flightAltitude;
            transform.position = newPosition;
        }
        else
        {
            _targetTransform = null;
            TargetReached?.Invoke();
        }
    }

    public void ApplySpeedBoost(int speedBoost, float duration)
    {
        StopBoostCoroutine();

        _speed = _baseSpeed + speedBoost;
        _speedBoostCoroutine = StartCoroutine(ResetSpeedAfterDelay(duration));
    }

    public void StopBoostCoroutine()
    {
        if (_speedBoostCoroutine != null)
            StopCoroutine(_speedBoostCoroutine);

        _speed = _baseSpeed;
    }

    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Slowed?.Invoke();
        _speed = _baseSpeed;
        _speedBoostCoroutine = null;
    }
}