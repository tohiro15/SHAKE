using UnityEngine;

public class Diamond : MonoBehaviour
{
    [Tooltip("Сколько алмазов даёт этот объект.")]
    [SerializeField] private int _diamondValue = 1;
    [SerializeField] private LayerMask _playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            var survivalMgr = GameManager.Instance.LevelManager.levelInfo.survivalModeManager;
            survivalMgr.Diamonds += _diamondValue;
            Destroy(gameObject);
        }
    }

}
