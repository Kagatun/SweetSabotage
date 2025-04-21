using Spawner;
using System.Collections.Generic;
using Timer;
using UnityEngine;
using Utility;

namespace Game
{
    public class StarterGame : MonoBehaviour
    {
        [SerializeField] private List<GridGenerator> _gridGenerators;
        [SerializeField] private List<SpawnerGeese> _spawnersGeese;
        [SerializeField] private SpawnerCookies _spawnerCookies;
        [SerializeField] private SpawnerTeleportFigures _spawnerTeleportFigures;
        [SerializeField] private BoostsGenerator _boostsGenerator;
        [SerializeField] private Timeline _timeline;
        [SerializeField] private ColorPenaltyCalculator _colorPenalty;
        [SerializeField] private CellCounter _cellCounter;
        [SerializeField] private AudioSource _music;

        private void Start()
        {
            for (int i = 0; i < _gridGenerators.Count; i++)
                _gridGenerators[i].CreateGrid();

            _spawnerCookies.StartSpawn();

            for (int i = 0; i < _gridGenerators.Count; i++)
                _boostsGenerator.SetSpawnPoints(_gridGenerators[i].GetMoveTransforms());

            _boostsGenerator.InstallBoosts();

            for (int i = 0; i < _gridGenerators.Count; i++)
            {
                _spawnersGeese[i].FillSpawnPoints(_gridGenerators[i].GetCornerTransforms());
                _spawnersGeese[i].FillMovePoints(_gridGenerators[i].GetMoveTransforms());
                _spawnersGeese[i].CreateGeese();
                _colorPenalty.SetGeese(_spawnersGeese[i].GetGeese());
            }


            for (int i = 0; i < _gridGenerators.Count; i++)
                _spawnerTeleportFigures.FillCells(_gridGenerators[i].GetValidCells());

            _spawnerTeleportFigures.SpawnFigures();
            _timeline.StartTimer();

            for (int i = 0; i < _gridGenerators.Count; i++)
                _cellCounter.SetCells(_gridGenerators[i].GetValidCells());

            _music.Play();
        }
    }
}
