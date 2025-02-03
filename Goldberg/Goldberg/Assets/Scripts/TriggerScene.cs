using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TriggerScene : MonoBehaviour
{
    [Tooltip("전환할 씬 이름")]
    public string targetSceneName; // 전환할 씬 이름

    [Tooltip("Trigger에 반응할 오브젝트의 태그 (비워두면 모든 오브젝트에 반응)")]
    public string targetTag; // 특정 태그를 가진 오브젝트와만 반응
    public GravityController gravityController;

    [Tooltip("씬 전환 전 대기 시간")]
    public float delayTime = 1f; // 이미지 표시 시간
    public float requiredTime = 3.0f; // 트리거 활성화 대기 시간

    [Tooltip("전환 전 잠시 표시할 이미지")]
    public Image transitionImage; // UI Image 컴포넌트
    public Sprite newSprite; // 전환 전에 표시할 이미지

    private bool flag = false;
    private float since = 0.0f;

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
    }

    void Update()
    {
        if (flag)
        {
            since += Time.deltaTime;
            if (since > requiredTime)
            {
                StartCoroutine(TransitionWithImage());
                flag = false; // 중복 실행 방지
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gravityController != null && gravityController.gravityEnabled)
        {
            // 태그 확인 (태그가 설정되어 있다면)
            if (!string.IsNullOrEmpty(targetTag) && !collision.CompareTag(targetTag))
            {
                Debug.Log($"{collision.name} does not match the required tag: {targetTag}");
                return;
            }

            flag = true; // 트리거 활성화
            since = 0.0f; // 타이머 초기화
        }
    }

    private IEnumerator TransitionWithImage()
    {
        if (transitionImage != null && newSprite != null)
        {
            // 이미지 변경
            transitionImage.sprite = newSprite;
            transitionImage.gameObject.SetActive(true); // 이미지 활성화

            // 대기 시간
            yield return new WaitForSeconds(delayTime);
        }

        // 씬 전환
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("Target scene name is not set in the Inspector!");
        }
    }
}
