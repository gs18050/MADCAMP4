using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButtonController: MonoBehaviour
{
    public Button reloadButton; // Reload 버튼

    void Start()
    {
        if (reloadButton != null)
        {
            reloadButton.onClick.AddListener(ReloadScene);
        }
        else
        {
            Debug.LogError("Reload Button is not assigned in the Inspector!");
        }
    }

    public void ReloadScene()
    {
        // 현재 씬을 다시 로드
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Reloading Scene: {currentSceneName}");
        SceneManager.LoadScene(currentSceneName);
    }
}
