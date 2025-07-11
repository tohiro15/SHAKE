using UnityEngine;

public class HPRecover : MonoBehaviour
{
    [Tooltip("Сколько HP даёт этот объект.")]
    [SerializeField] private int _recoveryCount = 2;
    [SerializeField] private LayerMask _playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            Combat player = other.gameObject.GetComponent<Combat>();
            player.AddHP(_recoveryCount);

            Destroy(gameObject);
        }
    }
}
