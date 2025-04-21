using Pastry;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Spawner
{
    public class SpawnerCookies : SpawnerObjects<Cookie>
    {
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private int _maxCookieCount = 35;

        private int _currentCookieCount = 0;
        private int _currentSpawnIndex = 0;
        private float _nextSpawnTime;
        private bool _isSpawning = false;

        private void Start()
        {
            _nextSpawnTime = Time.time + _spawnDelay;
        }

        public void StartSpawn() =>
            _isSpawning = true;

        protected override void OnRelease(Cookie cookie)
        {
            base.OnRelease(cookie);
            cookie.Cleaned -= OnCookieCleaned;
        }

        private void Update()
        {
            if (_isSpawning && _currentCookieCount < _maxCookieCount && Time.time >= _nextSpawnTime)
            {
                Transform spawnPoint = GetNextSpawnPoint();
                Color randomColor = ColorPalette.GetRandomActiveColor();

                SpawnCookie(spawnPoint.position, spawnPoint.rotation, randomColor);

                _nextSpawnTime = Time.time + _spawnDelay;
            }
        }

        private Transform GetNextSpawnPoint()
        {
            Transform spawnPoint = _spawnPoints[_currentSpawnIndex];
            _currentSpawnIndex++;

            if (_currentSpawnIndex >= _spawnPoints.Count)
                _currentSpawnIndex = 0;

            return spawnPoint;
        }

        private void SpawnCookie(Vector3 position, Quaternion rotation, Color color)
        {
            Cookie cookie = Get();
            cookie.Init(this);
            cookie.transform.position = position;
            cookie.transform.rotation = rotation;
            cookie.SetColor(color);
            cookie.DisableMover();
            cookie.PushStart();

            cookie.Cleaned += OnCookieCleaned;

            _currentCookieCount++;
        }

        private void OnCookieCleaned() =>
            _currentCookieCount--;
    }
}