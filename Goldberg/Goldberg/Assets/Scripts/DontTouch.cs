using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontTouch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Reloading Scene: {currentSceneName}");
        SceneManager.LoadScene(currentSceneName);
    }
}
