using System.Collections.Generic;
using UnityEngine;

public class SpawnerTeleportFigures : SpawnerObjects<TeleporterFigure>
{
    [SerializeField] private List<Transform> _spawnPoints;

    private List<Cell> _cells = new List<Cell>();
    private int _currentFigureCount = 0;

    protected override void OnGet(TeleporterFigure teleporterFigure)
    {
        base.OnGet(teleporterFigure);
        teleporterFigure.Used += OnFillShapes;
        teleporterFigure.SetStandardSize();
    }

    protected override void OnRelease(TeleporterFigure teleporterFigure)
    {
        base.OnRelease(teleporterFigure);
        teleporterFigure.Used -= OnFillShapes;
    }

    public void SpawnFigures()
    {
        foreach (Transform spawnPoint in _spawnPoints)
        {
            Color randomColor = ColorPalette.GetRandomActiveColor();
            SpawnFigure(spawnPoint, randomColor);
        }
    }

    private void SpawnFigure(Transform position, Color color)
    {
        TeleporterFigure teleporterFigure = Get();
        teleporterFigure.Init(this);
        teleporterFigure.transform.position = position.position;
        teleporterFigure.SetSpawnPoint(position.position, position.rotation);
        teleporterFigure.SetColor(color);
        teleporterFigure.FillListCells(_cells);
        teleporterFigure.DisableDetector();
        teleporterFigure.ResetStatusRemove();
        teleporterFigure.SetSmallSize();
        teleporterFigure.SetStatusInstall();

        _currentFigureCount++;
    }

    public void FillCells(List<Cell> cells) =>
        _cells = cells;

    private void OnFillShapes()
    {
        _currentFigureCount--;

        if (_currentFigureCount == 0)
            SpawnFigures();
    }
}