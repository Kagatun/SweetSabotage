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
        
        public void SetParameters(List<bool> activeColors)
        {
            s_activeColors = new List<Color>();

            for (int i = 0; _colorData.Count > i; i++)
            {
                _colorData[i].SetStatus(activeColors[i]);
                
                if (activeColors[i])
                    s_activeColors.Add(_colorData[i].Color);
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