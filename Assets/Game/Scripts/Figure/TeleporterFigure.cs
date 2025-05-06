using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using Common;
using Cells;

namespace Figure
{
    [RequireComponent(typeof(CookieStorage), typeof(CellDetector), typeof(MeshRenderer))]
    public class TeleporterFigure : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _effects;
        [SerializeField] private AudioSource _soundTeleport;
        [SerializeField] private AudioSource _soundError;
        [SerializeField] private AudioSource _soundRotate;
        [SerializeField] private AudioSource _soundInstall;
        [SerializeField] private Vector3 _offsetPosition;

        private CookieStorage _storage;
        private CellDetector _cellDetector;
        private MeshRenderer _render;
        private ColorInitializer _colorInitializer;

        private IPoolAdder<TeleporterFigure> _poolAdder;

        private Coroutine _coroutineRemove;
        private Vector3 _startSize;
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private WaitForSeconds _wait;
        private float _time = 0.15f;

        public event Action<TeleporterFigure> Removed;
        public event Action Used;

        public Color Color => _colorInitializer.Color;
        public Vector3 OffsetPosition => _offsetPosition;
        public bool IsInstall { get; private set; }
        public CookieStorage CookieStorage => _storage;

        private void Awake()
        {
            _storage = GetComponent<CookieStorage>();
            _cellDetector = GetComponent<CellDetector>();
            _render = GetComponent<MeshRenderer>();
            _colorInitializer = new ColorInitializer();
            _startSize = transform.localScale;
            _wait = new WaitForSeconds(_time);
        }

        public void Remove()
        {
            EnableDetector();

            for (int i = 0; i < _cellDetector.DetectedCells.Count; i++)
                _cellDetector.DetectedCells[i].ResetRequired();

            _cellDetector.ClearCells();
            _cellDetector.ClearCellsDetected();
            _storage.Clear();
            ResetPosition();
            Removed?.Invoke(this);
            _poolAdder.AddToPool(this);
        }

        public void Use() =>
            Used?.Invoke();

        public void Init(IPoolAdder<TeleporterFigure> poolAdder) =>
            _poolAdder = poolAdder;

        public void SetSmallSize()
        {
            float divider = 2;
            transform.localScale /= divider;
        }

        public void FillListCells(List<Cell> cells) =>
            _cellDetector.FillListCells(cells);

        public void EnableDetector()
        {
            _cellDetector.enabled = true;
            _cellDetector.SetColor(Color);
        }

        public void MarkAsNotInstalled() =>
            IsInstall = false;

        public void DisableDetector() =>
            _cellDetector.enabled = false;

        public void ResetPosition()
        {
            transform.position = _startPosition;
            transform.rotation = _startRotation;
        }

        public void Rotation()
        {
            int degreeRotation = 90;
            transform.Rotate(0, degreeRotation, 0);
            _soundRotate.Play();
        }

        public void SetSpawnPoint(Vector3 startPosition, Quaternion rotation)
        {
            _startPosition = startPosition;
            _startRotation = rotation;
        }

        public void SetStandardSize() =>
            transform.localScale = _startSize;

        public void SetColor(Color color)
        {
            _colorInitializer.SetColor(_render, color);
            _cellDetector.SetColor(color);
        }

        public void InstallPanelInCells()
        {
            if (_cellDetector.CanInstall())
            {
                _cellDetector.ClearCells();
                _cellDetector.ReserveCells();

                IsInstall = true;
                SetStandardSize();
                transform.position = _cellDetector.FirstDetectedCell.transform.position;
                Use();
                _soundInstall.Play();
            }
        }

        public void StartCoroutineRemove() =>
            _coroutineRemove = StartCoroutine(StartRemove());

        public void StopCoroutineRemove()
        {
            if (_coroutineRemove != null)
            {
                StopCoroutine(_coroutineRemove);
                _coroutineRemove = null;
            }
        }

        private IEnumerator StartRemove()
        {
            _soundTeleport.Play();

            foreach (var effect in _effects)
                effect.Play();

            yield return _wait;

            if (_storage.AreAllHoldersFilled())
                Remove();
            else
                _soundError.Play();
        }
    }
}