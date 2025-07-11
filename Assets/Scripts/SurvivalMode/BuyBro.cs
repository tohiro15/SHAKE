using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyBro : MonoBehaviour
{
    [SerializeField] private GameObject _broPrefab;
    [SerializeField] private int _price;

    [Space]

    [SerializeField] private Button _buyButton;
    [SerializeField] private Text _buyPrice;

    private void Start()
    {
        if (_broPrefab == null) return;

        _buyPrice.text = $"{_price}";

        _buyButton?.onClick.RemoveAllListeners();
        _buyButton?.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (GameManager.Instance.LevelManager.levelInfo.survivalModeManager.Diamonds < _price) return;
        GameManager.Instance.LevelManager.levelInfo.survivalModeManager.Diamonds -= _price;
        _price += _price;

        _buyPrice.text = $"{_price}";

        Vector3 target = GameManager.Instance.LevelManager.Player.transform.position;
        GameObject bro = Instantiate(_broPrefab);
        bro.transform.position = target;

    }
}
