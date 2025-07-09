using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Canvas UiCanvas;


    [Header("HUD")]
    [Space]

	[SerializeField] private GameObject hudPanel;
	[SerializeField] private Image healthBarImage;
	[SerializeField] private Text targetCountText;
	[SerializeField] private Text remainCountText;

	private Material healthBarMat;

    [Header("Defeat")]
    [Space]

	[SerializeField] private CanvasGroup defeatCanvasGroup;
	[SerializeField] private Text defeatStringText;

	[Header("Pause Menu")]
	[Space]

	[SerializeField] private Canvas _pauseCanvas;
	private bool _pauseGame = false;

    [Header("Animation")]
    [Space]

	[SerializeField]
	private AnimationCurve overShowCurve;

    [Header("Other")]
    [Space]

    public Color redColor;

	public Color greenColor;

	[SerializeField]
	private CanvasGroup successCanvasGroup;


	[SerializeField]
	private float overShowTime;

	private float overShowTimer;

	private Color tempColor;

	private bool overIsSuccess;

	private bool over;
    private void Awake()
	{
		healthBarMat = healthBarImage.material;
		_pauseGame = false;
    }

	public void Init(Camera _camera)
	{
		UiCanvas.worldCamera = _camera;
		UiCanvas.planeDistance = 1f;
		defeatCanvasGroup.alpha = 0f;
		defeatCanvasGroup.gameObject.SetActive(value: false);
		successCanvasGroup.alpha = 0f;
		successCanvasGroup.gameObject.SetActive(value: false);
		hudPanel.SetActive(value: true);
		over = false;
		overShowTimer = 0f;
	}

	public void UpdateCount(int _teamCount, int _remainCount, int _targetCount)
	{
		remainCountText.text = _remainCount.ToString();
		targetCountText.text = _teamCount.ToString() + "/" + _targetCount.ToString();
		if (_teamCount >= _targetCount)
		{
			targetCountText.color = greenColor;
		}
		else
		{
			targetCountText.color = redColor;
		}
	}

	private void Update()
	{
		if (GameManager.Instance.LevelManager.Player != null)
		{
			healthBarMat.SetFloat("_CutValue", GameManager.Instance.LevelManager.Player.Combat.HealthPercent);
		}
		if (over && overShowTimer < overShowTime)
		{
			overShowTimer += Time.unscaledDeltaTime;
			overShowTimer = Mathf.Min(overShowTimer, overShowTime);
			if (!overIsSuccess)
			{
				defeatCanvasGroup.alpha = overShowCurve.Evaluate(overShowTimer / overShowTime);
			}
			else
			{
				successCanvasGroup.alpha = overShowCurve.Evaluate(overShowTimer / overShowTime);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseMenu();
        }
    }

	public void Defeat(string _defeatString)
	{
		overIsSuccess = false;
		defeatStringText.text = _defeatString;
		hudPanel.SetActive(value: false);
		over = true;
		defeatCanvasGroup.gameObject.SetActive(value: true);
	}

	public void Success()
	{
		overIsSuccess = true;
		hudPanel.SetActive(value: false);
		over = true;
		successCanvasGroup.gameObject.SetActive(value: true);
	}

    public void ChangeTextAlpha(Text _text, float alpha)
	{
		tempColor = _text.color;
		tempColor.a = alpha;
		_text.color = tempColor;
	}

	public void PauseMenu()
	{
		if (_pauseCanvas == null) return;

		_pauseGame = !_pauseGame;
		_pauseCanvas.gameObject.SetActive(_pauseCanvas);

		LevelManager.Paused = !LevelManager.Paused;
    }
}
