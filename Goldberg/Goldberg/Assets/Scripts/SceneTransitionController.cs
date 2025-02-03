using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public GameObject gameEndObject; // GameEnd 오브젝트
    public GameObject junHoObject;  // JunHo 오브젝트
    public string nextSceneName = "NextScene"; // 전환할 씬 이름
    public float delayTime = 3f; // GameEnd 오브젝트 감지 후 대기 시간

    private bool gameEndTriggered = false; // GameEnd 감지 여부
    private float timer = 0f;              // 타이머

    void Start()
    {
        if (gameEndObject == null || junHoObject == null)
        {
            Debug.LogError("GameEnd or JunHo object is not assigned!");
        }
    }

    void Update()
    {
        // GameEnd가 트리거되고 타이머 시작
        if (gameEndTriggered)
        {
            timer += Time.deltaTime;

            if (timer >= delayTime)
            {
                Debug.Log("Transitioning to the next scene...");
                LoadNextScene();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // GameEnd 오브젝트 트리거 감지
        if (other.gameObject == gameEndObject)
        {
            Debug.Log("GameEnd triggered.");
            gameEndTriggered = true;
            timer = 0f; // 타이머 초기화
        }

        // JunHo 오브젝트 트리거 감지
        if (other.gameObject == junHoObject)
        {
            Debug.Log("JunHo contacted.");
            RestartScene();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // GameEnd와 JunHo가 접촉을 종료해도 아무 작업 없음
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not assigned!");
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
