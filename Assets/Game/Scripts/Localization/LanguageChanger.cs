using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

public class LanguageChanger : ButtonHandler
{
    [SerializeField] private int _indexLanguage;

    protected override void OnButtonClick()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_indexLanguage];

        YandexGame.savesData.IndexLanguage = _indexLanguage;
        YandexGame.SaveProgress();
    }
}
