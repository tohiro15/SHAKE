using UnityEngine;

public class PauseListener : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			LevelManager.Paused = !LevelManager.Paused;
		}
	}
}
