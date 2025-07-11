using System.Collections.Generic;
using Boost;
using UnityEngine;

namespace Spawner
{
    public class BoostsGenerator : MonoBehaviour
    {
        [SerializeField] private List<BoostCell> _boostCells;

        private List<Transform> _spawnPoints = new List<Transform>();
        private int _count;

        public void SetSpawnPoints(List<Transform> spawnPoints) =>
            _spawnPoints.AddRange(spawnPoints);

        public void InstallBoosts(int count)
        {
            _count = count;
            
            int boostsToSpawn = Mathf.Min(_count, _spawnPoints.Count);
            float offsetY = 0.001f;

            List<Transform> availableSpawnPoints = new List<Transform>(_spawnPoints);

            for (int i = 0; i < boostsToSpawn; i++)
            {
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];

                int randomBoostIndex = Random.Range(0, _boostCells.Count);
                BoostCell boost = _boostCells[randomBoostIndex];

                Instantiate(boost, spawnPoint.position + new Vector3(0, offsetY, 0), spawnPoint.rotation);
                availableSpawnPoints.RemoveAt(randomIndex);
            }
        }
    }
}