using System.Collections.Generic;
using UnityEngine;

public class CellDetector : MonoBehaviour
{
    [SerializeField] private List<Transform> _checkerPoints;

    private List<Cell> _cells = new List<Cell>();
    private List<Cell> _detectedCells = new List<Cell>();
    private List<Cell> _previouslyDetectedCells = new List<Cell>();

    private Color _color;
    private float _detectionRadius = 0.6f;
    private int _numberSections;

    public Cell FirstDetectedCell => _detectedCells[0];
    public IReadOnlyList<Cell> DetectedCells => _detectedCells;

    private void Awake()
    {
        _numberSections = _checkerPoints.Count;
    }

    private void Update()
    {
        _detectedCells.Clear();

        HashSet<Cell> occupiedCells = new HashSet<Cell>();

        for (int i = 0; i < _checkerPoints.Count; i++)
        {
            Transform point = _checkerPoints[i];
            Cell closestCell = FindClosestCell(point.position);

            if (closestCell != null)
            {
                float distanceSquared = (point.position - closestCell.transform.position).sqrMagnitude;

                if (distanceSquared <= _detectionRadius)
                {
                    if (closestCell.IsBusy == false && !occupiedCells.Contains(closestCell))
                    {
                        closestCell.SetColor(_color);
                        _detectedCells.Add(closestCell);
                        occupiedCells.Add(closestCell);
                    }
                }
            }
        }

        ResetCell();

        _previouslyDetectedCells = new List<Cell>(_detectedCells);
    }

    private void OnDisable()
    {
        ResetAllDetectedCells();
    }

    private Cell FindClosestCell(Vector3 pointPosition)
    {
        Cell closestCell = null;
        float closestDistanceSquared = float.MaxValue;

        foreach (var cell in _cells)
        {
            float distanceSquared = (pointPosition - cell.transform.position).sqrMagnitude;

            if (distanceSquared < closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestCell = cell;
            }
        }

        return closestCell;
    }

    public void ResetCell()
    {
        foreach (var cell in _previouslyDetectedCells)
        {
            if (_detectedCells.Contains(cell) == false)
                cell.ResetColor();
        }
    }

    public void ClearCells()
    {
        foreach (var cell in _detectedCells)
        {
            cell.ResetColor();
            cell.UnReserve();
        }
    }

    public void ClearCellsDetected() =>
        _detectedCells.Clear();

    public void SetColor(Color color) =>
        _color = color;

    public void FillListCells(List<Cell> cells) =>
        _cells = cells;

    private void ResetAllDetectedCells()
    {
        foreach (var cell in _previouslyDetectedCells)
            cell.ResetColor();

        _previouslyDetectedCells.Clear();
    }

    public bool CanInstall()
    {
        if (_detectedCells.Count != _numberSections)
            return false;

        HashSet<Cell> uniqueCells = new HashSet<Cell>(_detectedCells);

        if (uniqueCells.Count != _detectedCells.Count)
            return false;

        foreach (var cell in _detectedCells)
        {
            if (cell.IsBusy)
                return false;
        }

        return true;
    }

    public void ReserveCells()
    {
        foreach (var cell in _detectedCells)
            cell.Reserve();
    }
}