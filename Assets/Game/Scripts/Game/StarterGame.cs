using System.Collections.Generic;
using ManagementUtilities;
using Spawner;
using Timer;
using Unity;
using UnityEngine;
using Utility;

namespace Game
{
    public class StarterGame : MonoBehaviour
    {
        [SerializeField] private LoaderLevelGameSettings _levelGameSettings;
        [SerializeField] private List<Tutorial> _tutorials;
        [SerializeField] private ColorPalette _colorPalette;
        [SerializeField] private AudioClipInstaller _musicInstaller;
        [SerializeField] private List<Paint> _paints;
        [SerializeField] private List<GridGenerator> _gridGenerators;
        [SerializeField] private List<SpawnerGeese> _spawnersGeese;
        [SerializeField] private SpawnerCookies _spawnerCookies;
        [SerializeField] private SpawnerTeleportFigures _spawnerTeleportFigures;
        [SerializeField] private BoostsGenerator _boostsGenerator;
        [SerializeField] private Timeline _timeline;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private ColorPenaltyCalculator _colorPenalty;
        [SerializeField] private CellCounter _cellCounter;
        [SerializeField] private AudioSource _music;

        private void Start()
        {
            _colorPalette.SetParameters(_levelGameSettings.LevelSettings.ActiveСolors);

            for (int i = 0; i < _levelGameSettings.LevelSettings.GridGeneratorsSettings.Count; i++)
            {
                _gridGenerators[i].SetParameters(_levelGameSettings.LevelSettings.GridGeneratorsSettings[i]);
                _gridGenerators[i].CreateGrid();
            }

            _spawnerCookies.StartSpawn();

            for (int i = 0; i < _levelGameSettings.LevelSettings.GridGeneratorsSettings.Count; i++)
                _boostsGenerator.SetSpawnPoints(_gridGenerators[i].GetMoveTransforms());

            _boostsGenerator.InstallBoosts(_levelGameSettings.LevelSettings.BoostsCount);

            for (int i = 0; i < _levelGameSettings.LevelSettings.CountGeese.Count; i++)
            {
                _spawnersGeese[i].FillSpawnPoints(_gridGenerators[i].GetCornerTransforms());
                _spawnersGeese[i].FillMovePoints(_gridGenerators[i].GetMoveTransforms());
                _spawnersGeese[i].SpawnGeese(_levelGameSettings.LevelSettings.CountGeese[i]);
                _colorPenalty.SetGeese(_spawnersGeese[i].GetGeese());
            }

            for (int i = 0; i < _levelGameSettings.LevelSettings.GridGeneratorsSettings.Count; i++)
            {
                _spawnerTeleportFigures.SetCount(_levelGameSettings.LevelSettings.CountTeleportFigures);
                _spawnerTeleportFigures.SetParameters();
                _spawnerTeleportFigures.FillCells(_gridGenerators[i].GetValidCells());
            }

            _spawnerTeleportFigures.SpawnFigures();

            _timeline.SetTime(_levelGameSettings.LevelSettings.Time);
            _timeline.StartTimer();

            for (int i = 0; i < _gridGenerators.Count; i++)
                _cellCounter.SetCells(_gridGenerators[i].GetValidCells());

            _scoreCounter.SetParameters(_levelGameSettings.LevelSettings.MaxScore);

            _musicInstaller.SetMusic(_levelGameSettings.LevelSettings.NumberLevel);
            _music.Play();

            for (int i = 0; i < _levelGameSettings.LevelSettings.ActivePaints.Count; i++)
            {
                if (_levelGameSettings.LevelSettings.ActivePaints[i])
                    _paints[i].gameObject.SetActive(true);
            }

            if (_levelGameSettings.LevelSettings.HasActiveTutorial)
                _tutorials[_levelGameSettings.LevelSettings.IndexTutorial].gameObject.SetActive(true);
        }
    }
}