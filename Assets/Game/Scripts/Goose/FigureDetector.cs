using System;
using System.Collections.Generic;
using Figure;
using UnityEngine;

namespace Bird
{
    public class FigureDetector : MonoBehaviour
    {
        private List<TeleporterFigure> _figures = new List<TeleporterFigure>();

        public event Action<TeleporterFigure> FigureRemoved;

        public IReadOnlyList<TeleporterFigure> Figures => _figures;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out TeleporterFigure figure))
            {
                if (_figures.Contains(figure) == false && figure.IsInstall)
                {
                    _figures.Add(figure);
                    figure.Removed += OnFigureRemoved;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out TeleporterFigure figure))
            {
                RemoveFigure(figure);
                figure.Removed -= OnFigureRemoved;
            }
        }

        private void OnFigureRemoved(TeleporterFigure figure)
        {
            RemoveFigure(figure);
        }

        private void RemoveFigure(TeleporterFigure figure)
        {
            if (_figures.Contains(figure))
            {
                _figures.Remove(figure);
                figure.Removed -= OnFigureRemoved;
                FigureRemoved?.Invoke(figure);
            }
        }
    }
}