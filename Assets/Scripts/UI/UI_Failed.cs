using GamePush;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Failed : MonoBehaviour
{
    [Header("Failed")]
    [Space]

    [SerializeField] private GameObject _failedPanel;
    [SerializeField] private Button _returnButton;

    [Header("Attempt")]
    [Space]

    [SerializeField] private float _waitTime = 8f;
    [SerializeField] private GameObject _attemptPanel;
    [Space]
    [SerializeField] private Image _ringTimer;
    [SerializeField] public Button _adsButton;
    [SerializeField] public Button _skipButton;

    private void Start ()
    {
        if (!GameManager.Instance.LevelManager.levelInfo.survivalMode)
        {
            _attemptPanel.SetActive(false);
            _failedPanel.SetActive(true);
            return;
        }

        LevelManager.AttemptWait = true;

        _adsButton?.onClick.RemoveAllListeners();
        _adsButton?.onClick.AddListener(ShowFullscreen);

        _attemptPanel.SetActive(true);
        _failedPanel.SetActive(false);

        _returnButton?.onClick.RemoveAllListeners();
        _returnButton?.onClick.AddListener(ReloadLevel);

        _skipButton?.gameObject.SetActive(false);

        StartCoroutine(ShowSkipButton());
    }

    public void CloseADS()
    {
        _attemptPanel.SetActive(false);
        _failedPanel.SetActive(true);

        LevelManager.AttemptWait = false;
    }

    public void ShowFullscreen() => GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose); 
    private void OnFullscreenStart() => CloseADS();
    private void OnFullscreenClose(bool success)
    {
        CloseADS();
        //Вторая попытка выдается
    }

    private IEnumerator ShowSkipButton()
    {
        float elapsed = 0f;
        _ringTimer.fillAmount = 0f;

        while (elapsed < _waitTime)
        {
            elapsed += Time.unscaledDeltaTime;
            _ringTimer.fillAmount = Mathf.Clamp01(elapsed / _waitTime);

            yield return null;
        }

        _skipButton?.gameObject.SetActive(true);
        _skipButton?.onClick.RemoveAllListeners();
        _skipButton?.onClick.AddListener(CloseADS);
    }

    private void ReloadLevel()
    {
        GameManager.Instance.LevelManager.TryLoadLevel(GameManager.Instance.LevelManager.levelInfo.currentLevelIndex);
    }
}
