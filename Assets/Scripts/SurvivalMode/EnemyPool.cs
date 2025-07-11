using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    public int PoolSize = 20;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.SetActive(false);
            _pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {
        if (_pool.Count == 0)
        {
            GameObject extra = Instantiate(_enemyPrefab);
            extra.SetActive(false);
            _pool.Enqueue(extra);
            PoolSize++;
        }

        // Достаём из пула
        GameObject enemy = _pool.Dequeue();
        enemy.transform.position = position;
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        _pool.Enqueue(enemy);
    }
}
