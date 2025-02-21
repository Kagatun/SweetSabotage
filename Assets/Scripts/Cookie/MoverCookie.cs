using System;
using UnityEngine;

public class MoverCookie : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;

    private int _speed = 12;

    public event Action TargetReached;

    private void Update()
    {
        if (_targetTransform != null)
            GoToTarget(_targetTransform);
    }

    public void GoToTarget(Transform positionTarget)
    {
        _targetTransform = positionTarget;

        float minDistanceToTargetSqr = 0.29f;
        int flightAltitude = 1;

        Vector3 direction = (_targetTransform.position - transform.position);

        if (direction.sqrMagnitude > minDistanceToTargetSqr)
        {
            direction.Normalize();
            transform.forward = direction;

            Vector3 newPosition = transform.position + transform.forward * _speed * Time.deltaTime;
            newPosition.y = flightAltitude;
            transform.position = newPosition;
        }
        else
        {
            TargetReached?.Invoke();
            _targetTransform = null;
        }
    }
}