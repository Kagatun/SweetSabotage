using Boost;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public class BoostsGenerator : MonoBehaviour
    {
        [SerializeField] private List<BoostCell> _boostCells;
        [SerializeField] private int _count;

        private List<Transform> _spawnPoints = new List<Transform>();

        public void SetSpawnPoints(List<Transform> spawnPoints) =>
            _spawnPoints.AddRange(spawnPoints);

        public void InstallBoosts()
        {
            int boostsToSpawn = Mathf.Min(_count, _spawnPoints.Count);

            List<Transform> availableSpawnPoints = new List<Transform>(_spawnPoints);

            for (int i = 0; i < boostsToSpawn; i++)
            {
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];

                int randomBoostIndex = Random.Range(0, _boostCells.Count);
                BoostCell boost = _boostCells[randomBoostIndex];

                float offsetY = 0.51f;
                Instantiate(boost, spawnPoint.position + new Vector3(0, offsetY, 0), spawnPoint.rotation);
                availableSpawnPoints.RemoveAt(randomIndex);
            }
        }
    }
}
