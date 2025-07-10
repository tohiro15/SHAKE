using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool _pool;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnDelay = 5f;
    [SerializeField] private float _distanceToPlayer = 30f;
    [SerializeField] private float _minSpawnDistance = 1.5f;

    private List<GameObject> _activeEnemies = new List<GameObject>();
    private Transform _player;
    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);

            if (_activeEnemies.Count >= _pool.PoolSize)
                continue;

            if (_player == null && GameManager.Instance.LevelManager.Player != null)
            {
                _player = GameManager.Instance.LevelManager.Player.transform;
            }

            if (_player == null)
                continue;

            bool spawnSuccess = false;
            for (int attempt = 0; attempt < _spawnPoints.Length; attempt++)
            {
                Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

                float distanceToPlayer = Vector3.Distance(_player.position, spawnPoint.position);
                if (_distanceToPlayer > distanceToPlayer)
                    continue;

                GameObject enemy = _pool.GetEnemy(spawnPoint.position);
                if (enemy != null)
                {
                    _activeEnemies.Add(enemy);

                    Combat enemyScript = enemy.GetComponent<Combat>();
                    enemyScript.OnDeath += () =>
                    {
                        _pool.ReturnToPool(enemy);
                        _activeEnemies.Remove(enemy);
                    };

                    spawnSuccess = true;
                    break;
                }
            }

            if (!spawnSuccess)
            {
                Debug.Log("Нет подходящих точек для спавна");
            }
        }

    }
}
