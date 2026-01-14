using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject startButton;

    private void Start()
    {
        // 初期状態は非表示
        startButton.SetActive(false);

        // RobbyScene に入った時点で評価
        RefreshButtonVisibility();

        // ネットワーク開始時のコールバックを登録
        NetworkManager.Singleton.OnServerStarted += OnNetworkStarted;
    }

    private void OnDestroy()
    {
        // 終了時に解除（エラー防止）
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnNetworkStarted;
        }
    }

    private void OnNetworkStarted()
    {
        // ホストになった瞬間に呼ばれる
        if (NetworkManager.Singleton.IsHost)
        {
            startButton.SetActive(true);
        }
    }

    private void RefreshButtonVisibility()
    {
        if (NetworkManager.Singleton == null)
        {
            startButton.SetActive(false);
            return;
        }

        // Host のみ表示
        startButton.SetActive(NetworkManager.Singleton.IsHost);
    }

    public void OnClickStart()
    {
        Debug.Log("OK");
        if (!NetworkManager.Singleton.IsHost) return;

        NetworkManager.Singleton.SceneManager.LoadScene(
            "MainScene",
            UnityEngine.SceneManagement.LoadSceneMode.Single
        );
    }
}
