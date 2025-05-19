using System.Collections.Generic;
using Cells;
using Figure;
using UnityEngine;
using Utility;

namespace Spawner
{
    public class SpawnerTeleportFigures : SpawnerListObjects<TeleporterFigure>
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spacing = 3f;
        [SerializeField] private ColorHistoryTracker _colorTracker;

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
            int additional = 1;
            int numberOfColors = ColorPalette.GetActiveColorsCount() + additional;

            List<Transform> newSpawnPoints = CreateSpawnPointsAlongX(_spawnPoint, numberOfColors, _spacing);

            foreach (Transform spawnPoint in newSpawnPoints)
            {
                SpawnFigure(spawnPoint);
            }
        }

        private List<Transform> CreateSpawnPointsAlongX(Transform originalPoint, int numberOfPoints, float spacing)
        {
            List<Transform> spawnPoints = new List<Transform>();

            if (numberOfPoints == 1)
            {
                spawnPoints.Add(originalPoint);

                return spawnPoints;
            }

            float startOffset = -(numberOfPoints - 1) * spacing / 2f;

            for (int i = 0; i < numberOfPoints; i++)
            {
                float x = originalPoint.position.x + startOffset + i * spacing;

                GameObject spawnPoint = new GameObject("SpawnPoint_" + i);
                spawnPoint.transform.position = new Vector3(x, originalPoint.position.y, originalPoint.position.z);
                spawnPoint.transform.rotation = originalPoint.rotation;

                spawnPoints.Add(spawnPoint.transform);
            }

            return spawnPoints;
        }

        private void SpawnFigure(Transform position)
        {
            TeleporterFigure teleporterFigure = Get();
            Vector3 startPosition = position.position + teleporterFigure.OffsetPosition;
            teleporterFigure.Init(this);
            teleporterFigure.transform.position = startPosition;
            teleporterFigure.SetSpawnPoint(startPosition, position.rotation);
            teleporterFigure.SetColor(_colorTracker.GetNextColor());
            teleporterFigure.FillListCells(_cells);
            teleporterFigure.DisableDetector();
            teleporterFigure.SetSmallSize();
            teleporterFigure.MarkAsNotInstalled();

            _currentFigureCount++;
        }

        public void FillCells(List<Cell> cells) =>
            _cells.AddRange(cells);

        private void OnFillShapes()
        {
            _currentFigureCount--;

            if (_currentFigureCount == 0)
                SpawnFigures();
        }
    }
}