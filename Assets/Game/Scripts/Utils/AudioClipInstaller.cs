using System.Collections.Generic;
using UnityEngine;

namespace Unity
{
    public class AudioClipInstaller : MonoBehaviour
    {
        [SerializeField] private AudioSource _music;
        [SerializeField] private List<AudioClip> _audioClips;

        public void SetMusic(int index)
        {
            int numberOfLevelsLocation = 10;
            int clipIndex = index % numberOfLevelsLocation;
            _music.clip = _audioClips[clipIndex];
        }
    }
}