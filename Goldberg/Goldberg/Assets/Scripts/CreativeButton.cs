using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro를 사용하기 위한 네임스페이스
using UnityEngine.UI; // Button 사용을 위한 네임스페이스

public class CreativeButton : MonoBehaviour
{
    public string targetSceneName = "CreativeMode"; // 전환할 씬 이름
    public GravityController gravityController; // 중력 상태 확인
    public Button button; // Unity UI Button

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

        if (button == null)
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick); // 버튼 클릭 이벤트 연결
            }
            else
            {
                Debug.LogError("Button component not found!");
            }
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("Button clicked");

        // (1) 중력 상태 확인
        if (gravityController != null && gravityController.gravityEnabled)
        {
            Debug.Log("Gravity is enabled");

            // (2) 씬 전환
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
            Debug.Log("Gravity is not enabled");
        }
    }
}
