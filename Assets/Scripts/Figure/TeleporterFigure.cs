using System;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterFigure : MonoBehaviour
{
    private CookieStorage _storage;
    private CellDetector _cellDetector;
    private MeshRenderer _render;

    private IPoolAdder<TeleporterFigure> _poolAdder;

    private Vector3 _startSize;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    public event Action Used;

    public Color Color => _render.material.color;
    public bool IsInstall { get; private set; }
    public bool IsRemove { get; private set; }
    public CookieStorage CookieStorage => _storage;

    private void Awake()
    {
        _storage = GetComponent<CookieStorage>();
        _cellDetector = GetComponent<CellDetector>();
        _render = GetComponent<MeshRenderer>();
        _startSize = transform.localScale;
    }

    public void Remove()
    {
        EnableDetector();
        ClearCells();
        _storage.Clear();
        _poolAdder.AddToPool(this);
    }

    public void Use() =>
          Used?.Invoke();

    public void Init(IPoolAdder<TeleporterFigure> poolAdder) =>
        _poolAdder = poolAdder;

    public void SetSmallSize()
    {
        int divider = 3;
        transform.localScale /= divider;
    }

    public void FillListCells(List<Cell> cells) =>
        _cellDetector.FillListCells(cells);

    public void EnableDetector() =>
        _cellDetector.enabled = true;

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
    }

    public void SetSpawnPoint(Vector3 startPosition)
    {
        _startPosition = startPosition;
        _startRotation = transform.rotation;
    }

    public void SetStandardSize()
    {
        transform.localScale = _startSize;
        _startRotation = transform.rotation;
    }

    public void SetStatusRemove() =>
            IsRemove = true;

    public void ResetStatusRemove() =>
            IsRemove = false;

    public void SetColor(Color color)
    {
        _render.material.color = color;
        _cellDetector.SetColor(color);
    }

    public void InstallPanelInCells()
    {
        if (_cellDetector.CanInstall())
        {
            foreach (var cell in _cellDetector.DetectedCells)
                cell.Reserve();

            IsInstall = true;
            SetStandardSize();
            transform.position = _cellDetector.FirstDetectedCell.transform.position;
            Use();
        }
    }

    private void ClearCells() =>
    _cellDetector.ClearCells();
}