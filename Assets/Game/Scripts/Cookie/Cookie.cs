using System;
using Common;
using Figure;
using UnityEngine;

namespace Pastry
{
    [RequireComponent(typeof(Mover), typeof(MeshRenderer), typeof(Rigidbody))]
    public class Cookie : MonoBehaviour
    {
        private Mover _mover;
        private MeshRenderer _render;
        private IPoolAdder<Cookie> _poolAdder;
        private Rigidbody _rigidbody;
        private MeshCollider _collider;
        private PointHolder _point;
        private ColorInitializer _colorInitializer;

        private int _forcePush = 1;
        private Quaternion _rotation;

        public event Action Cleaned;

        public Color Color => _colorInitializer.Color;

        private void Awake()
        {
            int rotationZ = 90;
            _rotation = Quaternion.Euler(0, 0, rotationZ);

            _mover = GetComponent<Mover>();
            _render = GetComponent<MeshRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<MeshCollider>();
            _colorInitializer = new ColorInitializer();
        }

        private void OnEnable()
        {
            _mover.TargetReached += OnStopMove;
        }

        private void OnDisable()
        {
            _mover.TargetReached -= OnStopMove;
        }

        public void SetColor(Color color) =>
            _colorInitializer.SetColor(_render, color);

        public void Remove()
        {
            transform.parent = null;
            _poolAdder.AddToPool(this);
        }

        public void Init(IPoolAdder<Cookie> poolAdder) =>
            _poolAdder = poolAdder;

        public void EnableMover()
        {
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            _mover.enabled = true;
        }

        public void DisableMover()
        {
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            _mover.enabled = false;
        }

        public void PushStart()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(-transform.forward * _forcePush, ForceMode.Impulse);
        }

        public void SetTarget(PointHolder target)
        {
            Cleaned?.Invoke();
            _rigidbody.velocity = Vector3.zero;
            _collider.enabled = false;
            _point = target;
            _mover.enabled = true;
            _mover.GoToTarget(target.transform);
        }

        public void OnStopMove()
        {
            transform.position = _point.transform.position;
            transform.parent = _point.transform;
            transform.localRotation = _rotation;

            _mover.enabled = false;
            _collider.enabled = false;
            _rigidbody.isKinematic = true;

            _point.AddCookie(this);
            _point = null;
        }
    }
}