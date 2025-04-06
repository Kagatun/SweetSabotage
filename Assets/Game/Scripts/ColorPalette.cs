using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    [SerializeField] private List<ColorData> _colorData;

    private static List<Color> _activeColors;

    public static IReadOnlyList<Color> ActiveColors => _activeColors;

    private void Awake()
    {
        _activeColors = new List<Color>();

        foreach (var colorData in _colorData)
        {
            if (colorData.IsActive)
                _activeColors.Add(colorData.Color);
        }
    }

    public static int GetActiveColorsCount() =>
        _activeColors.Count;

    public static Color GetRandomActiveColor()
    {
        int randomIndex = Random.Range(0, _activeColors.Count);

        return _activeColors[randomIndex];
    }
}