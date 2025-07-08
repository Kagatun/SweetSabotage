using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Spawner
{
    public class ColorHistoryTracker : MonoBehaviour
    {
        private List<Color> _availableColors;
        private List<Color> _spawnedColorsHistory = new List<Color>();
        private int _checkAfterCount;
        private int _number = 3;

        public void SetParameters()
        {
            _availableColors = new List<Color>(ColorPalette.ActiveColors);
            _checkAfterCount = _availableColors.Count * _number;
        }

        public Color GetNextColor()
        {
            /*_availableColors = new List<Color>(ColorPalette.ActiveColors);
            _checkAfterCount = _availableColors.Count * _number;*/
            
            if (_spawnedColorsHistory.Count >= _checkAfterCount)
            {
                var missingColors = FindMissingColors();

                if (missingColors.Count > 0)
                {
                    Color missingColor = missingColors[0];
                    _spawnedColorsHistory.Add(missingColor);

                    return missingColor;
                }
                else
                {
                    _spawnedColorsHistory.Clear();
                }
            }

            Color randomColor = ColorPalette.GetRandomActiveColor();
            _spawnedColorsHistory.Add(randomColor);

            return randomColor;
        }

        private List<Color> FindMissingColors()
        {
            List<Color> missingColors = new List<Color>();

            foreach (var color in _availableColors)
            {
                if (_spawnedColorsHistory.Contains(color) == false)
                {
                    missingColors.Add(color);
                }
            }

            return missingColors;
        }
    }
}