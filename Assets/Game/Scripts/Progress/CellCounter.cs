using System.Collections.Generic;
using Cells;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class CellCounter : MonoBehaviour
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _imageCheck;
        [SerializeField] private Image _imageDefeat;

        private List<Cell> _cells = new List<Cell>();
        private int _currentNumber;

        public bool IsCountComplete => _currentNumber >= _cells.Count;

        private void OnEnable()
        {
            _scoreCounter.Filled += OnEnableImageDefeat;
        }

        private void OnDisable()
        {
            _scoreCounter.Filled -= OnEnableImageDefeat;
        }

        public void SetCells(List<Cell> cells)
        {
            _cells.AddRange(cells);
            RecalculateCells();
            DisplayInvoice();
        }

        private void RecalculateCells()
        {
            HashSet<Cell> validCells = new HashSet<Cell>();

            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i].IsRequired)
                    validCells.Add(_cells[i]);
            }

            if (validCells.Count == 0)
                gameObject.SetActive(false);

            foreach (var cell in _cells)
                cell.Counted -= AddCount;

            _cells.Clear();

            foreach (var cell in validCells)
            {
                if (_cells.Contains(cell) == false)
                {
                    _cells.Add(cell);
                    cell.Counted += AddCount;
                }
            }
        }

        private void DisplayInvoice() =>
            _text.text = $"{_cells.Count} / {_currentNumber}";

        private void AddCount()
        {
            _currentNumber++;

            if (_cells.Count <= _currentNumber)
                _imageCheck.gameObject.SetActive(true);

            DisplayInvoice();
        }

        private void OnEnableImageDefeat()
        {
            if (_cells.Count > _currentNumber)
                _imageDefeat.gameObject.SetActive(true);
        }
    }
}
