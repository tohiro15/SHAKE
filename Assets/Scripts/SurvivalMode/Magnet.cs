using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePush;

public class Magnet : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    [Tooltip("Скорость перемещения алмаза к магниту")]
    [SerializeField] private float _suckSpeed = 10f;
    [Tooltip("Расстояние до центра магнита, при котором считается, что алмаз достиг магнита")]
    [SerializeField] private float _collectDistance = 0.5f;
    [Tooltip("Моделька магнита")]
    [SerializeField] private GameObject _model;
    private bool _isActivated = false;
    private List<Diamond> _targets;
    private int _collectedCount = 0;

    private Collider _triggerCollider;

    private void Awake()
    {
        _triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated)
            return;

        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            ActivateMagnet();
        }
    }

    private void ActivateMagnet()
    {
        _isActivated = true;
        _triggerCollider.enabled = false;
        _model.SetActive(false);

        var all = FindObjectsOfType<Diamond>();
        _targets = new List<Diamond>(all.Length);
        foreach (var d in all)
            if (d != null && d.gameObject.activeInHierarchy)
                _targets.Add(d);

        StartCoroutine(SuckRoutine());
    }

    private IEnumerator SuckRoutine()
    {
        var survivalMgr = GameManager.Instance.LevelManager.levelInfo.survivalModeManager;

        while (_targets.Count > 0)
        {
            for (int i = _targets.Count - 1; i >= 0; i--)
            {
                var diamond = _targets[i];
                if (diamond == null)
                {
                    _targets.RemoveAt(i);
                    continue;
                }

                Transform dt = diamond.transform;
                dt.position = Vector3.MoveTowards(dt.position, GameManager.Instance.LevelManager.Player.transform.position, _suckSpeed * Time.deltaTime);

                if (Vector3.Distance(dt.position, GameManager.Instance.LevelManager.Player.transform.position) <= _collectDistance)
                {
                    survivalMgr.Diamonds += diamond.DiamondValue;
                    _collectedCount++;

                    Destroy(diamond.gameObject);
                    _targets.RemoveAt(i);
                }
            }

            yield return null;
        }
    }
}
