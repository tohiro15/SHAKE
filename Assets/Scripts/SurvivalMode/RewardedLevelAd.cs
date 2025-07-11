using GamePush;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardedLevelAd : MonoBehaviour
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private LayerMask _playerLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            GP_System.IsDev();
            ShowFullscreen();
        }
    }
    public void ShowFullscreen() => GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);
    private void OnFullscreenStart() => Debug.Log("ON FULLSCREEN AD START");
    private void OnFullscreenClose(bool success) => GameManager.Instance.LevelManager.LoadLevel(_levelIndex);
}
