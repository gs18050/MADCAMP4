using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro를 사용하기 위한 네임스페이스

public class StartButtonController : MonoBehaviour
{
    public string targetSceneName; // 전환할 씬 이름
    public GravityController gravityController; // 중력 상태 확인
    public TextMeshPro textMeshPro; // "Start" 텍스트 확인

    void Start()
    {
        if (gravityController == null)
        {
            gravityController = FindObjectOfType<GravityController>();
            if (gravityController == null)
            {
                Debug.LogError("GravityController not found in the scene!");
            }
        }

        if (textMeshPro == null)
        {
            textMeshPro = GetComponentInChildren<TextMeshPro>();
            if (textMeshPro == null)
            {
                Debug.LogError("TextMeshPro component not found in children!");
            }
        }
    }

    void OnMouseDown()
    {
        Debug.Log("StartButton clicked");

        // (1) 중력 상태 확인
        if (gravityController != null && gravityController.gravityEnabled)
        {
            Debug.Log("Gravity is enabled");

            // (2) 텍스트가 "Start"인지 확인
            if (textMeshPro != null && textMeshPro.text == "Start")
            {
                Debug.Log("Text is 'Start'");

                // (3) 씬 전환
                if (!string.IsNullOrEmpty(targetSceneName))
                {
                    SceneManager.LoadScene(targetSceneName);
                }
                else
                {
                    Debug.LogError("Target scene name is not assigned!");
                }
            }
            else
            {
                Debug.Log("Text is not 'Start'");
            }
        }
        else
        {
            Debug.Log("Gravity is not enabled");
        }
    }
}
