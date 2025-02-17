using System.Collections.Generic;
using UnityEngine;

public class CellDetector : MonoBehaviour
{
    private List<Transform> _points = new List<Transform>();
    private List<Cell> _cells = new List<Cell>();
    private List<Cell> _detectedCells = new List<Cell>();
    private List<Cell> _previouslyDetectedCells = new List<Cell>();
    private Color _color;
    private float _detectionRadius = 0.6f;

    public IReadOnlyList<Cell> DetectedCells => _detectedCells;
    public Cell FirstDetectedCell => _detectedCells.Count > 0 ? _detectedCells[0] : null;

    private void Update()
    {
        _detectedCells.Clear();

        for (int i = 0; i < _points.Count; i++)
        {
            Transform point = _points[i];
            Cell closestCell = FindClosestCell(point.position);

            if (closestCell != null)
            {
                float distanceSquared = (point.position - closestCell.transform.position).sqrMagnitude;

                if (distanceSquared <= _detectionRadius)
                {
                    closestCell.SetColor(_color);
                    _detectedCells.Add(closestCell);
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

    public void FillPoints(List<Transform> points) =>
        _points.AddRange(points);

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
}