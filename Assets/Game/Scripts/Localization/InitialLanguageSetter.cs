using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;
using YG;
using System.Runtime.InteropServices;

public class InitialLanguageSetter : MonoBehaviour
{
    const string USA = "en";
    const string China = "zh";
    const string France = "fr";
    const string Germany = "de";
    const string India = "hi";
    const string Japan = "ja";
    const string Korea = "ko";
    const string Brazil = "pt";
    const string Turkey = "tr";

    [DllImport("__Internal")]
    private static extern string GetBrowserLanguage();

    private bool _isLocalizationReady = false;
    private bool _isYandexDataReady = false;

    private void OnEnable()
    {
        YandexGame.GetDataEvent += OnYandexDataLoaded;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= OnYandexDataLoaded;
    }

    private void Start()
    {
        StartCoroutine(InitializeLocalization());

        if (YandexGame.SDKEnabled)
            OnYandexDataLoaded();
    }

    public int GetLanguage()
    {
        string currentLanguage = GetBrowserLanguage();

        int indexRussia = 0;
        int indexUSA = 1;
        int indexChina = 2;
        int indexFrance = 3;
        int indexGermany = 4;
        int indexIndia = 5;
        int indexJapan = 6;
        int indexKorea = 7;
        int indexBrazil = 8;
        int indexTurkey = 9;

        switch (currentLanguage)
        {
            case USA:
                return indexUSA;

            case China:
                return indexChina;

            case France:
                return indexFrance;

            case Germany:
                return indexGermany;

            case India:
                return indexIndia;

            case Japan:
                return indexJapan;

            case Korea:
                return indexKorea;

            case Brazil:
                return indexBrazil;

            case Turkey:
                return indexTurkey;

            default:
                return indexRussia;
        }
    }

    private void OnYandexDataLoaded()
    {
        _isYandexDataReady = true;
        TrySetLanguage();
    }

    private void TrySetLanguage()
    {
        if (_isLocalizationReady && _isYandexDataReady)
        {
            if (YandexGame.savesData.IsFirstEntrance)
                LoadLanguage();
            else
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[YandexGame.savesData.IndexLanguage];
        }
    }

    private void LoadLanguage()
    {
        int currentLanguageIndex = GetLanguage();
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLanguageIndex];

        YandexGame.savesData.IsFirstEntrance = false;
        YandexGame.savesData.IndexLanguage = currentLanguageIndex;
        YandexGame.SaveProgress();
    }

    private IEnumerator InitializeLocalization()
    {
        yield return LocalizationSettings.InitializationOperation;

        _isLocalizationReady = true;
        OnYandexDataLoaded();
    }
}
