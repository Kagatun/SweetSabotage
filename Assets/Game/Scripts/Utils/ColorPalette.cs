using System.Collections.Generic;
using Spawner;
using UnityEngine;

namespace Utility
{
    public class ColorPalette : MonoBehaviour
    {
        private static List<Color> s_activeColors;

        public static IReadOnlyList<Color> ActiveColors => s_activeColors;
        
        [SerializeField] private List<ColorData> _colorData;
        
        private void Awake()
        {
            s_activeColors = new List<Color>();

            foreach (var colorData in _colorData)
            {
                if (colorData.IsActive)
                    s_activeColors.Add(colorData.Color);
            }
        }

        public static int GetActiveColorsCount() =>
            s_activeColors.Count;

        public static Color GetRandomActiveColor()
        {
            int randomIndex = Random.Range(0, s_activeColors.Count);

            return s_activeColors[randomIndex];
        }
    }
}