using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goose : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _render;

    private GooseColor _gooseColor;

    public Color Color => _gooseColor.Color;

    private void Awake()
    {
        _gooseColor = new GooseColor();
    }

    public void SetStartColor(Color color) =>
        _gooseColor.SetColor(_render, color);
}
