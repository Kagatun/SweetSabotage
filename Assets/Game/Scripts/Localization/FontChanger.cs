using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine;
using YG;

public class FontChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private List<TMP_FontAsset> _fonts;
    [SerializeField] private List<FontSettings> _fontSettings;

    private int _currentIndex;

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            SetFont();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += SetFont;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= SetFont;
    }

    public void SetFont()
    {
        var currentLocale = LocalizationSettings.SelectedLocale;
        int currentIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(currentLocale);
        _currentIndex = currentIndex;

        _text.font = _fonts[_currentIndex];

        ResetTextSettings();
        ApplyFontSettings(_fontSettings[_currentIndex]);
    }

    private void ApplyFontSettings(FontSettings settings)
    {
        if (settings.IsBold)
            _text.fontStyle = FontStyles.Bold;

        _text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, settings.FaceDilate);
        _text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, settings.OutlineWidth);
    }

    private void ResetTextSettings()
    {
        _text.fontStyle = FontStyles.Normal;
        _text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
        _text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0f);
    }
}