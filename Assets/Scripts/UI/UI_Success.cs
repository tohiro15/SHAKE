using UnityEngine;
using UnityEngine.UI;

public class UI_Success : MonoBehaviour
{
    [SerializeField] private Button _returnButton;
    private LevelManager _levelManager;
    private void Start()
    {
        _levelManager = GameManager.Instance.LevelManager; 

        _returnButton?.onClick.RemoveAllListeners();
        _returnButton?.onClick.AddListener(LoadLevel);

        LevelManager.AttemptWait = true;
    }

    private void LoadLevel()
    {
        if (_levelManager.levelInfo.hasNextLevel)
        {
            LevelManager.AttemptWait = false;
            _levelManager.TryLoadLevel(_levelManager.LevelIndex + 1);
        }
        else
        {
            _levelManager.TryLoadLevel(0);
        }
    }
}
