using System.Collections.Generic;
using UnityEngine;

public class SpawnerCookies : SpawnerObjects<Cookie>
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _spawnDelay;

    private readonly int MaxCookieCount = 35;

    private int _currentCookieCount = 0;
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
        if (_isSpawning && _currentCookieCount < MaxCookieCount && Time.time >= _nextSpawnTime)
        {
            Transform spawnPoint = GetNextSpawnPoint();
            Color randomColor = ColorPalette.GetRandomActiveColor();

            SpawnCookie(spawnPoint.position, spawnPoint.rotation, randomColor);

            _nextSpawnTime = Time.time + _spawnDelay;
        }
    }

    private Transform GetNextSpawnPoint() =>
         _spawnPoints[Random.Range(0, _spawnPoints.Count)];

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