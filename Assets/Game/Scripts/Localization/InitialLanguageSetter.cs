using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

namespace Localization
{
    public class InitialLanguageSetter : MonoBehaviour
    {
       private const string Russia = "ru";
       private const string USA = "en";
       private const string China = "zh";
       private const string France = "fr";
       private const string Germany = "de";
       private const string India = "hi";
       private const string Japan = "ja";
       private const string Korea = "ko";
       private const string Brazil = "pt";
       private const string Turkey = "tr";

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
            string currentLanguage = YandexGame.EnvironmentData.language;

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
                case Russia:
                    return indexRussia;

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
                    return indexUSA;
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
}
