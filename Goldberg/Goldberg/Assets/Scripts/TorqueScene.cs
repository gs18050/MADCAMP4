using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TorqueScene : MonoBehaviour
{
    public HingeJoint2D hingeJoint; // Hinge Joint 2D를 가진 오브젝트
    public float torqueThreshold = 10f; // 다음 씬으로 넘어갈 토크 임계값
    public string nextSceneName; // 전환할 씬 이름

    public Image transitionImage; // UI Image 컴포넌트
    public Sprite newSprite; // 전환 전에 표시할 이미지
    public float delayTime = 1f; // 대기 시간

    private Rigidbody2D rigidbody2D;

    void Start()
    {
        if (hingeJoint == null)
        {
            Debug.LogError("HingeJoint2D is not assigned in the Inspector!");
            return;
        }

        // Hinge Joint가 붙어있는 Rigidbody2D 가져오기
        rigidbody2D = hingeJoint.GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D not found on the object with HingeJoint2D!");
        }
    }

    void Update()
    {
        if (hingeJoint == null || rigidbody2D == null)
            return;

        // Hinge Joint에 걸리는 토크 계산
        float currentTorque = Mathf.Abs(rigidbody2D.angularVelocity * rigidbody2D.inertia);

        // 토크가 임계값을 초과하면 씬 전환
        if (currentTorque >= torqueThreshold)
        {
            Debug.Log($"Torque exceeded threshold: {currentTorque}. Loading next scene...");
            StartCoroutine(ShowImageAndLoadScene());
        }
    }

    private IEnumerator ShowImageAndLoadScene()
    {
        if (transitionImage != null && newSprite != null)
        {
            // 새 이미지 표시
            transitionImage.sprite = newSprite;
            transitionImage.gameObject.SetActive(true);

            // 일정 시간 대기
            yield return new WaitForSeconds(delayTime);

            // 씬 전환
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("Next scene name is not assigned!");
            }
        }
        else
        {
            Debug.LogError("Transition image or new sprite is not assigned!");
        }
    }
}
