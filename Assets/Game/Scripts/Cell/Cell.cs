using System;
using UnityEngine;

namespace Cells
{
    public class Cell : MonoBehaviour
    {
        private MeshRenderer _render;
        private Color _startColor;
        private Color _startRequiredColor;

        public event Action Counted;

        public Color CurrentColor { get; private set; }
        public bool IsBusy { get; private set; }
        public bool IsRequired { get; private set; }

        private void Awake()
        {
            _render = GetComponent<MeshRenderer>();
            CurrentColor = _render.material.color;
            _startColor = _render.material.color;
        }

        public void SetColor(Color color) =>
            _render.material.color = color;

        public void SetRequiredColor(Color color) =>
            _startRequiredColor = color;

        public void ResetColor()
        {
            if (IsRequired == false)
                _render.material.color = _startColor;
            else
                _render.material.color = _startRequiredColor;
        }

        public void SetRequired() =>
            IsRequired = true;

        public void ResetRequired()
        {
            if (IsRequired)
            {
                IsRequired = false;
                Counted?.Invoke();
            }
        }

        public void Reserve() =>
            IsBusy = true;

        public void UnReserve() =>
            IsBusy = false;
    }
}
