using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterGame : MonoBehaviour
{
    [SerializeField] private GridGenerator _gridGenerator;
    [SerializeField] private SpawnerCookies _spawnerCookies;
    [SerializeField] private SpawnerGeese _spawnerGeese;
    [SerializeField] private SpawnerTeleportFigures _spawnerTeleportFigures;

    private void Start()
    {
        _gridGenerator.CreateGrid();
        _spawnerCookies.StartSpawn();

        _spawnerGeese.FillSpawnPoints(_gridGenerator.GetCornerTransforms());
        _spawnerGeese.CreateGeese();

        _spawnerTeleportFigures.FillCells(_gridGenerator.GetValidCells());
        _spawnerTeleportFigures.SpawnFigures();
    }
}
