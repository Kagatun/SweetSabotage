using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private MeshRenderer _render;

    public Color CurrentColor { get; private set; }
    public Color StartColor { get; private set; }
    public bool IsBusy { get; private set; }

    private void Awake()
    {
        _render = GetComponent<MeshRenderer>();
        CurrentColor = _render.material.color;
        StartColor = _render.material.color;
    }

    public void SetColor(Color color) =>
        _render.material.color = color;

    public void ResetColor() =>
        _render.material.color = StartColor;

    public void Reserve() =>
        IsBusy = true;

    public void UnReserve() =>
        IsBusy = false;
}
