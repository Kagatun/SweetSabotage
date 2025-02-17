using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCookies : SpawnerObjects<Cookie>
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _spawnDelay;

    private readonly int MaxCookieCount = 35;

    private Coroutine _spawnCoroutine;
    private int _currentSpawnIndex = 0;
    private int _currentCookieCount = 0;
    private WaitForSeconds _wait;

    private void Start()
    {
        _wait = new WaitForSeconds(_spawnDelay);
    }

    public void StartSpawn() =>
        StartSpawningIfNeeded();

    protected override void OnRelease(Cookie cookie)
    {
        base.OnRelease(cookie);
        cookie.Cleaned -= OnCookieCleaned;
    }

    private IEnumerator SpawnCookiesRoutine()
    {
        while (_currentCookieCount < MaxCookieCount)
        {
            Transform spawnPoint = GetNextSpawnPoint();
            Color randomColor = ColorPalette.GetRandomActiveColor();

            SpawnCookie(spawnPoint.position, randomColor);

            yield return _wait;
        }

        StopSpawn();
    }

    private void StopSpawn()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private Transform GetNextSpawnPoint()
    {
        Transform spawnPoint = _spawnPoints[_currentSpawnIndex];
        _currentSpawnIndex = (_currentSpawnIndex + 1) % _spawnPoints.Count;

        return spawnPoint;
    }

    private void SpawnCookie(Vector3 position, Color color)
    {
        if (_currentCookieCount >= MaxCookieCount)
            return;

        Cookie cookie = Get();
        cookie.Init(this);
        cookie.transform.position = position;
        cookie.SetColor(color);
        cookie.PushStart();

        cookie.Cleaned += OnCookieCleaned;

        _currentCookieCount++;
    }

    private void OnCookieCleaned()
    {
        _currentCookieCount--;
        StartSpawningIfNeeded();
    }

    private void StartSpawningIfNeeded()
    {
        if (_currentCookieCount < MaxCookieCount)
            _spawnCoroutine = StartCoroutine(SpawnCookiesRoutine());
    }
}