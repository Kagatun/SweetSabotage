using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

namespace Utility
{
    public class SoundLoudnessChanger : MonoBehaviour
    {
        private const string MasterVolume = "MasterVolume";
        private const string MusicVolume = "MusicVolume";
        private const string EffectsVolume = "EffectsVolume";

        [SerializeField] private AudioMixerGroup _mixerMaster;
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _effectsVolume;

        private float _master;
        private float _music;
        private float _effects;

        private void Start()
        {
            if (YandexGame.SDKEnabled)
                LoadVolume();
        }

        private void OnEnable()
        {
            _masterVolume.onValueChanged.AddListener(ChangeVolumeMaster);
            _musicVolume.onValueChanged.AddListener(ChangeVolumeMusic);
            _effectsVolume.onValueChanged.AddListener(ChangeVolumeEffects);
            YandexGame.GetDataEvent += LoadVolume;
        }

        private void OnDisable()
        {
            _masterVolume.onValueChanged.RemoveListener(ChangeVolumeMaster);
            _musicVolume.onValueChanged.RemoveListener(ChangeVolumeMusic);
            _effectsVolume.onValueChanged.RemoveListener(ChangeVolumeEffects);
            YandexGame.GetDataEvent -= LoadVolume;
        }

        private void ChangeVolumeMaster(float volume)
        {
            _mixerMaster.audioMixer.SetFloat(MasterVolume, GetMinValue(volume) * 20);
            _master = volume;
            YandexGame.savesData.MasterVolume = _master;
            YandexGame.SaveProgress();
        }

        private void ChangeVolumeMusic(float volume)
        {
            _mixerMaster.audioMixer.SetFloat(MusicVolume, GetMinValue(volume) * 20);
            _music = volume;
            YandexGame.savesData.MusicVolume = _music;
            YandexGame.SaveProgress();
        }

        private void ChangeVolumeEffects(float volume)
        {
            _mixerMaster.audioMixer.SetFloat(EffectsVolume, GetMinValue(volume) * 20);
            _effects = volume;
            YandexGame.savesData.EffectsVolume = _effects;
            YandexGame.SaveProgress();
        }

        private float GetMinValue(float volume)
        {
            return Mathf.Log10(Mathf.Clamp(volume, 0.00001f, 1f));
        }

        private void LoadVolume()
        {
            _master = YandexGame.savesData.MasterVolume;
            _music = YandexGame.savesData.MusicVolume;
            _effects = YandexGame.savesData.EffectsVolume;

            _masterVolume.value = _master;
            _musicVolume.value = _music;
            _effectsVolume.value = _effects;

            ChangeVolumeMaster(_master);
            ChangeVolumeMusic(_music);
            ChangeVolumeEffects(_effects);
        }
    }
}
