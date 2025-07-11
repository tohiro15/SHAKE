using UnityEngine;
using UnityEngine.UI;

public class WaveCountHUD : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public Text text;

    private void Update()
    {
        if ((bool)GameManager.Instance.LevelManager.levelInfo.survivalMode)
        {
            canvasGroup.alpha = 1f;
            text.text = $"{GameManager.Instance.LevelManager.levelInfo.survivalModeManager.CurrentWave}";
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
}
