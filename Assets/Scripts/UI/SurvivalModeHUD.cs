using UnityEngine;
using UnityEngine.UI;

public class SurvivalModeHUD : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public Text text;

    private void Update()
    {
        if ((bool)GameManager.Instance.LevelManager.levelInfo.survivalMode)
        {
            canvasGroup.alpha = 1f;
            //text.text = ;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
}
