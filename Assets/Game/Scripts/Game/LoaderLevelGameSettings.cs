using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using YG;

public class LoaderLevelGameSettings : MonoBehaviour
{
    [SerializeField] private List<GameSettings> _settings;

    public GameSettings LevelSettings => _settings[YandexGame.savesData.LevelNumber];
}