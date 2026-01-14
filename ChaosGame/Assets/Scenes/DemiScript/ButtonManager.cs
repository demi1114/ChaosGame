using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Hostのみ表示するボタン")]
    [SerializeField] private GameObject button;

    [Header("遷移先シーン名")]
    [SerializeField] private string targetSceneName;

    private void Start()
    {
        button.SetActive(false);

        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsHost)
        {
            button.SetActive(true);
        }
        exitButton.SetActive(false);

        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsHost)
        {
            exitButton.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        }
    }

    private void OnServerStarted()
    {
        // Host のみボタン表示
        if (NetworkManager.Singleton.IsHost)
        {
            button.SetActive(true);
        }
    }

    public void OnClick()
    {
        // 念のためガード
        if (!NetworkManager.Singleton.IsHost) return;

        NetworkManager.Singleton.SceneManager.LoadScene(
            targetSceneName,
            LoadSceneMode.Single);
    }

    [Header("Hostのみ表示する終了ボタン")]
    [SerializeField] private GameObject exitButton;
    public void OnClickExit()
    {
        // Hostのみ
        if (NetworkManager.Singleton == null ||
            !NetworkManager.Singleton.IsHost)
            return;

        Debug.Log("Game Quit (Host)");

        // 全クライアント切断
        NetworkManager.Singleton.Shutdown();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
