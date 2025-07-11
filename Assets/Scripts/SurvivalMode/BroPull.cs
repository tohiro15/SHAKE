using System.Collections.Generic;
using UnityEngine;

public class BroPool : MonoBehaviour
{
    [SerializeField] private GameObject _broPrefab;
    [SerializeField] private int _poolSize = 20;
    public int PoolSize => _poolSize;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bro = Instantiate(_broPrefab);
            bro.SetActive(false);
            _pool.Enqueue(bro);
        }
    }

    public GameObject GetBro(Vector3 position)
    {
        if (_pool.Count > 0)
        {
            GameObject bro = _pool.Dequeue();
            bro.transform.position = position;
            bro.SetActive(true);
            return bro;
        }

        return null;
    }

    public void ReturnToPool(GameObject bro)
    {
        bro.SetActive(false);
        _pool.Enqueue(bro);
    }
}
