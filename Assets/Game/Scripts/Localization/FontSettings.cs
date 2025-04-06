using UnityEngine;

[System.Serializable]
public class FontSettings
{
    [SerializeField] private bool _isBold = false;
    [SerializeField] private float _faceDilate;
    [SerializeField] private float _outlineWidth;

    public bool IsBold => _isBold;
    public float FaceDilate => _faceDilate;
    public float OutlineWidth => _outlineWidth;
}