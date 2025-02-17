using System;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterFigure : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;

    private List<Cell> _cells;
    private IPoolAdder<TeleporterFigure> _poolAdder;
    private Vector3 _startSize;
    private CellDetector _cellDetector;
    private MeshRenderer _render;
    private int _numberSections;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    public event Action Used;

    public Color Color => _render.material.color;
    public bool isInstall { get; private set; }

    private void Awake()
    {
        _cellDetector = GetComponent<CellDetector>();
        _cellDetector.FillPoints(_points);
        _render = GetComponent<MeshRenderer>();
        _startSize = transform.localScale;
        _numberSections = _points.Count;
    }

    public void Remove()
    {
        _poolAdder.AddToPool(this);
        Used?.Invoke();
    }

    public void Init(IPoolAdder<TeleporterFigure> poolAdder) =>
        _poolAdder = poolAdder;

    public void SetSmallSize()
    {
        int divider = 3;
        transform.localScale /= divider;
    }

    public void FillListCells(List<Cell> cells)
    {
        _cells = cells;
        _cellDetector.FillListCells(_cells);
    }

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

    public void SetStandardSize() =>
            transform.localScale = _startSize;

    public void SetColor(Color color)
    {
        _render.material.color = color;
        _cellDetector.SetColor(color);
    }

    public void InstallPanelInCells()
    {
        if (CanInstall())
        {
            foreach (var cell in _cellDetector.DetectedCells)
                cell.Reserve();

            isInstall = true;
            SetStandardSize();
            transform.position = _cellDetector.FirstDetectedCell.transform.position;
            Used?.Invoke();
        }
    }

    private bool CanInstall()
    {
        if (_cellDetector.DetectedCells.Count != _numberSections)
            return false;

        foreach (var cell in _cellDetector.DetectedCells)
        {
            if (cell.IsBusy)
                return false;
        }

        return true;
    }
}