using GamePush;
using UnityEngine;
using UnityEngine.UI;

public class UI_SurvivalMode : MonoBehaviour
{
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private Button _shopButton;

    [SerializeField] private CanvasGroup canvasGroup;
    private bool _isOpen;
    private void Start()
    {
        _isOpen = false;
        _shopPanel.SetActive(false);

        if (GP_Device.IsDesktop())
        {
            _shopButton.gameObject.SetActive(false);
        }
        else if (GP_Device.IsMobile())
        {
            _shopButton.gameObject.SetActive(true);

            _shopButton?.onClick.RemoveAllListeners();
            _shopButton?.onClick.AddListener(ShopMenu);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && GP_Device.IsDesktop())
        {
            ShopMenu();
        }

        if ((bool)GameManager.Instance.LevelManager.levelInfo.survivalMode)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
    public void ShopMenu()
    {
        if (_shopPanel == null) return;

        _isOpen = !_isOpen;
        LevelManager.Paused = !LevelManager.Paused;

        _shopPanel.SetActive(_isOpen);
    }
}
