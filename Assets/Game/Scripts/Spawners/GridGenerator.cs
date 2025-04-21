using Cells;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private float _offset;
        [SerializeField] private Transform _centralPointSpawn;
        [SerializeField] private int _brokenCellsCount;
        [SerializeField] private int _requiredCellsCount;
        [SerializeField] private Color _colorRequired;

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
                            _cornerTransforms.Add(cell.transform);
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

            CreateBrokenCells();
            CreateRequiredCells();
        }

        private void CreateBrokenCells()
        {
            if (_brokenCellsCount <= 0 || _brokenCellsCount >= _cells.Count)
                return;

            List<int> availableIndices = new List<int>();

            for (int i = 0; i < _cells.Count; i++)
                availableIndices.Add(i);

            for (int i = 0; i < _brokenCellsCount; i++)
            {
                int randomIndex = Random.Range(0, availableIndices.Count);
                int cellIndex = availableIndices[randomIndex];
                availableIndices.RemoveAt(randomIndex);

                Cell brokenCell = _cells[cellIndex];
                MakeCellBroken(brokenCell);
            }
        }

        private void CreateRequiredCells()
        {
            int actualRequiredCount = Mathf.Min(_requiredCellsCount, _cells.Count - _brokenCellsCount);
            List<Cell> availableCells = new List<Cell>();

            foreach (var cell in _cells)
            {
                if (!cell.IsBusy)
                {
                    availableCells.Add(cell);
                }
            }

            for (int i = 0; i < actualRequiredCount; i++)
            {
                if (availableCells.Count == 0)
                    break;

                int randomIndex = Random.Range(0, availableCells.Count);
                Cell requiredCell = availableCells[randomIndex];
                availableCells.RemoveAt(randomIndex);

                MakeCellRequired(requiredCell);
            }
        }

        private void MakeCellBroken(Cell cell)
        {
            cell.Reserve();
            cell.SetColor(Color.black);
        }

        private void MakeCellRequired(Cell cell)
        {
            cell.SetRequired();
            cell.SetRequiredColor(_colorRequired);
            cell.SetColor(_colorRequired);
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
            return new List<Cell>(_cells);
        }

        public List<Transform> GetMoveTransforms()
        {
            List<Transform> orderedTransforms = new List<Transform>(_emptyCellTransforms);
            Vector3 center = Vector3.zero;

            foreach (var transform in orderedTransforms)
                center += transform.position;

            center /= orderedTransforms.Count;

            orderedTransforms.Sort((transformA, transformB) =>
            {
                float angleA = Mathf.Atan2(transformA.position.z - center.z, transformA.position.x - center.x);
                float angleB = Mathf.Atan2(transformB.position.z - center.z, transformB.position.x - center.x);

                return angleA.CompareTo(angleB);
            });

            return orderedTransforms;
        }

        public List<Transform> GetCornerTransforms()
        {
            return new List<Transform>(_cornerTransforms);
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
}