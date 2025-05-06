using UnityEngine;
using UnityEngine.UI;
using YG;
using System;
using System.Collections.Generic;
using InterfaceUI;

public class YandexSliderController : ButtonHandler
{
    [SerializeField] private List<YandexSlider> _sliders = new List<YandexSlider>();

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            LoadSettings();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        foreach (var slider in _sliders)
            slider.Slider.onValueChanged.AddListener(value => SaveValue(slider.SaveField.ToString(), value));
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (var slider in _sliders)
            slider.Slider.onValueChanged.RemoveAllListeners();
    }

    private void SaveValue(string fieldName, float value)
    {
        if (YandexGame.savesData == null)
            return;

        var field = typeof(SavesYG).GetField(fieldName);

        if (field != null && field.FieldType == typeof(float))
        {
            field.SetValue(YandexGame.savesData, value);
            YandexGame.SaveProgress();
        }
    }

    private void LoadSettings()
    {
        if (YandexGame.savesData == null)
            return;

        foreach (var slider in _sliders)
        {
            if (slider.Slider == null)
                continue;

            var field = typeof(SavesYG).GetField(slider.SaveField.ToString());

            if (field != null && field.FieldType == typeof(float))
                slider.Slider.value = (float)field.GetValue(YandexGame.savesData);
        }
    }

    protected override void OnButtonClick()
    {
        foreach (var slider in _sliders)
        {
            if (slider.Slider == null)
                continue;

            slider.Slider.value = slider.DefaultValue;
            SaveValue(slider.SaveField.ToString(), slider.DefaultValue);
        }
    }
}

[Serializable]
public class YandexSlider
{
    public YandexSaveField SaveField;
    public Slider Slider;
    public float DefaultValue;
    public bool InvertDefault;
}

public enum YandexSaveField
{
    OffsetFigureHorizontal,
    OffsetFigureVertical,
    OffsetPaintLeftHorizontal,
    OffsetPaintLeftVertical,
    OffsetPaintRightHorizontal,
    OffsetPaintRightVertical
}