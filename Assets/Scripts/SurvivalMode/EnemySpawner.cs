using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Space]
    [SerializeField] private Transform[] _spawnZones;
    [SerializeField] private float _spawnDelay = 5f;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _distanceToPlayer = 30f;

    [Header("Enemy Settings")]
    [Space]
    [SerializeField] private EnemyPool _pool;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _maxEnemiesThisWave = 5;
    [SerializeField] private int _increaseEnemy = 2;

    [Header("Drop Settings")]
    [Space]
    [SerializeField] private float _nothingDropChance = 20f;
    [SerializeField] private GameObject _diamondPrefab;
    [SerializeField] private float _diamondDropChance = 90;
    [SerializeField] private GameObject _hpRecoverPrefab;
    [SerializeField] private float _hpRecoverDropChance = 50;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _bombDropChance = 10;

    private Transform _player;
    private bool _waitingNextWave = false;
    private bool _fullWave = false;
    private int _spawnedThisWave = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);

            CleanActiveEnemies();

            if (_waitingNextWave)
                continue;

            if (_player == null && GameManager.Instance.LevelManager.Player != null)
                _player = GameManager.Instance.LevelManager.Player.transform;

            if (_player == null)
                continue;

            if (_spawnedThisWave >= _maxEnemiesThisWave)
                _fullWave = true;

            if (_fullWave)
                continue;

            bool spawnSuccess = false;

            for (int attempt = 0; attempt < _spawnZones.Length; attempt++)
            {
                Transform zone = _spawnZones[Random.Range(0, _spawnZones.Length)];
                Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
                Vector3 spawnPos = zone.position + new Vector3(randomOffset.x, 0, randomOffset.y);

                if (Vector3.Distance(_player.position, spawnPos) < _distanceToPlayer)
                    continue;

                if (Physics.OverlapSphere(spawnPos, 1.5f, _enemyLayer).Length > 0)
                    continue;

                GameObject enemy = _pool.GetEnemy(spawnPos);
                if (enemy != null)
                {
                    _spawnedThisWave++;

                    GameManager.Instance.LevelManager.levelInfo.survivalModeManager.ActiveEnemies.Add(enemy);

                    Combat enemyScript = enemy.GetComponent<Combat>();
                    enemyScript.OnDeath += () =>
                    {
                        Vector3 dropPos = enemy.transform.position;

                        float total = _nothingDropChance + _hpRecoverDropChance + _diamondDropChance;

                        float roll = Random.value * total;

                        if (roll < _nothingDropChance) { }
                        else if (roll < _nothingDropChance + _hpRecoverDropChance) Instantiate(_hpRecoverPrefab, dropPos, Quaternion.identity);
                        else if(roll < _nothingDropChance + _hpRecoverDropChance + _bombDropChance) Instantiate(_bombPrefab, dropPos, Quaternion.identity);
                        else Instantiate(_diamondPrefab, dropPos, Quaternion.identity);

                        _pool.ReturnToPool(enemy);
                        GameManager.Instance.LevelManager.levelInfo.survivalModeManager.ActiveEnemies.Remove(enemy);
                        if (GameManager.Instance.LevelManager.levelInfo.survivalModeManager.ActiveEnemies.Count == 0)
                            StartCoroutine(NextWaveDelay());
                    };

                    spawnSuccess = true;
                    break;
                }
            }

            if (!spawnSuccess)
            {
                Debug.Log("Не удалось найти свободное место для спавна");
            }
        }
    }

    private IEnumerator NextWaveDelay()
    {
        _waitingNextWave = true;

        Debug.Log("Все враги побеждены. Следующая волна скоро начнётся...");

        CleanActiveEnemies();

        yield return new WaitForSeconds(3f);

        GameManager.Instance.LevelManager.levelInfo.survivalModeManager.CurrentWave += 1;
        _maxEnemiesThisWave += _increaseEnemy;

        _fullWave = false;
        _spawnedThisWave = 0;
        _waitingNextWave = false;
    }

    private void CleanActiveEnemies()
    {
        var list = GameManager.Instance.LevelManager.levelInfo.survivalModeManager.ActiveEnemies;
        list.RemoveAll(e => e == null);
    }

}
