using GamePush;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attempt : MonoBehaviour
{
    [SerializeField] private float _waitTime = 8f;
    [Space]
    [SerializeField] private GameObject _attemptPanel;
    [SerializeField] private GameObject _failedPanel;
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
            // добавляем реальное (unscaled) время
            elapsed += Time.unscaledDeltaTime;
            _ringTimer.fillAmount = Mathf.Clamp01(elapsed / _waitTime);

            // ждём следующий кадр (корутина не блокируется даже при timeScale = 0)
            yield return null;
        }

        // показаем кнопку, когда время вышло
        _skipButton?.gameObject.SetActive(true);
        _skipButton?.onClick.RemoveAllListeners();
        _skipButton?.onClick.AddListener(CloseADS);
    }


}
