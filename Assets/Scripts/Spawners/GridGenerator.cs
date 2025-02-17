using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private float _offset;
    [SerializeField] private Transform _centralPointSpawn;

    private List<Cell> _cells = new List<Cell>();
    private List<Cell> _cellsDelete = new List<Cell>();
    private List<Transform> _emptyCellTransforms = new List<Transform>();
    private List<Transform> _cornerTransforms = new List<Transform>();
    public void CreateGrid()
    {
        var cellSize = _cellPrefab.GetComponent<MeshRenderer>().bounds.size;

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Vector3 localPosition = new Vector3(x * (cellSize.x + _offset), 0, y * (cellSize.z + _offset));
                Cell cell = Instantiate(_cellPrefab, localPosition, Quaternion.identity, _centralPointSpawn);

                if (IsEdgeCell(x, y))
                {
                    ClearCellComponents(cell);
                    _cellsDelete.Add(cell);
                    _emptyCellTransforms.Add(cell.transform);

                    if (IsCornerCell(x, y))
                    {
                        _cornerTransforms.Add(cell.transform);
                    }
                }
                else
                {
                    _cells.Add(cell);
                }
            }
        }

        List<Cell> allCells = new List<Cell>();
        allCells.AddRange(_cells);
        allCells.AddRange(_cellsDelete);

        Vector3 centerOfMass = CalculateCenterOfMass(allCells);

        AlignCellsToParent(allCells, centerOfMass);

        _cellsDelete.Clear();
    }

    private bool IsEdgeCell(int x, int y)
    {
        return x == 0 || x == _gridSize.x - 1 || y == 0 || y == _gridSize.y - 1;
    }

    private bool IsCornerCell(int x, int y)
    {
        return (x == 0 && y == 0) ||
               (x == 0 && y == _gridSize.y - 1) ||
               (x == _gridSize.x - 1 && y == 0) ||
               (x == _gridSize.x - 1 && y == _gridSize.y - 1);
    }

    private void ClearCellComponents(Cell cell)
    {
        Destroy(cell.GetComponent<Collider>());
        Destroy(cell.GetComponent<Rigidbody>());
        Destroy(cell.GetComponent<Cell>());
        Destroy(cell.GetComponent<MeshRenderer>());
    }

    public List<Cell> GetValidCells()
    {
        return _cells;
    }

    public List<Transform> GetCornerTransforms()
    {
        return _cornerTransforms;
    }

    private Vector3 CalculateCenterOfMass(List<Cell> cells)
    {
        Vector3 sumPositions = Vector3.zero;

        foreach (var cell in cells)
            sumPositions += cell.transform.position;

        return sumPositions / cells.Count;
    }

    private void AlignCellsToParent(List<Cell> cells, Vector3 centerOfMass)
    {
        Vector3 offset = _centralPointSpawn.position - centerOfMass;

        foreach (var cell in cells)
            cell.transform.position += offset;
    }
}