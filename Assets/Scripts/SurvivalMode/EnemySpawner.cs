using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header ("Spawn Settings")]
    [Space]

    [SerializeField] private Transform[] _spawnZones;
    [SerializeField] private float _spawnDelay = 5f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _distanceToPlayer = 30f;

    [Header("Enemy Settings")]
    [Space]

    [SerializeField] private EnemyPool _pool;
    [SerializeField] private LayerMask _enemyLayer;

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
                _player = GameManager.Instance.LevelManager.Player.transform;

            if (_player == null)
                continue;

            bool spawnSuccess = false;

            for (int attempt = 0; attempt < _spawnZones.Length; attempt++)
            {
                Transform zone = _spawnZones[Random.Range(0, _spawnZones.Length)];
                Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
                Vector3 spawnPos = zone.position + new Vector3(randomOffset.x, 0, randomOffset.y);

                float distanceToPlayer = Vector3.Distance(_player.position, spawnPos);
                if (distanceToPlayer < _distanceToPlayer)
                    continue;

                Collider[] colliders = Physics.OverlapSphere(spawnPos, 1.5f, _enemyLayer);
                if (colliders.Length > 0)
                    continue;

                GameObject enemy = _pool.GetEnemy(spawnPos);
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
                Debug.Log("Ќе удалось найти свободное место дл€ спавна");
            }
        }
    }
}
