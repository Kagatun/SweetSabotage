using Bird;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Spawner
{
    public class SpawnerGeese : SpawnerObjects<Goose>
    {
        [SerializeField] private int _countGeese;

        private List<Color> _usedColors = new List<Color>();
        private List<Transform> _spawnPoints = new List<Transform>();
        private List<Transform> _movePoints = new List<Transform>();
        private List<Goose> _geese = new List<Goose>();

        public void CreateGeese() =>
            SpawnGeese();

        public void FillSpawnPoints(List<Transform> spawnPoints) =>
            _spawnPoints = spawnPoints;

        public void FillMovePoints(List<Transform> movePoints) =>
            _movePoints = movePoints;

        public List<Goose> GetGeese() =>
            new List<Goose>(_geese);

        public void RemoveAllGoose()
        {
            for (int i = 0; i < _geese.Count; i++)
                _geese[i].Remove();
        }

        private void SpawnGeese()
        {
            int availableColorsCount = ColorPalette.GetActiveColorsCount();
            _countGeese = Mathf.Min(_countGeese, availableColorsCount, _spawnPoints.Count);

            for (int i = 0; i < _countGeese; i++)
                SpawnGoose();
        }

        private Transform GetRandomSpawnPoint()
        {
            if (_spawnPoints.Count == 0)
                return null;

            int randomIndex = Random.Range(0, _spawnPoints.Count);
            Transform spawnPoint = _spawnPoints[randomIndex];
            _spawnPoints.RemoveAt(randomIndex);

            return spawnPoint;
        }

        private Color GetUniqueColor()
        {
            Color color;

            do
            {
                color = ColorPalette.GetRandomActiveColor();
            } while (_usedColors.Contains(color));

            _usedColors.Add(color);

            return color;
        }

        private Goose SpawnGoose()
        {
            Goose goose = Get();

            Transform spawnPoint = GetRandomSpawnPoint();

            if (spawnPoint != null)
            {
                goose.transform.position = spawnPoint.position;
                goose.transform.forward = spawnPoint.forward;

                Color uniqueColor = GetUniqueColor();
                goose.SetColor(uniqueColor);
                goose.SetStartMovePoints(_movePoints);
                goose.Init(this);
                _geese.Add(goose);
            }

            return goose;
        }
    }
}
