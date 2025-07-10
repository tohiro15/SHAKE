using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _poolSize = 20;
    public int PoolSize => _poolSize; 
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.SetActive(false);
            _pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {
        if (_pool.Count > 0)
        {
            GameObject enemy = _pool.Dequeue();
            enemy.transform.position = position;
            enemy.SetActive(true);
            return enemy;
        }

        return null;
    }

    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        _pool.Enqueue(enemy);
    }
}
