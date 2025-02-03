using UnityEngine;
using System.Collections.Generic;

public class GravityController : MonoBehaviour
{
    private Dictionary<Rigidbody2D, float> originalGravityScales = new Dictionary<Rigidbody2D, float>();
    public bool gravityEnabled = false;

    void Start()
    {
        // 씬에 있는 모든 Rigidbody2D 가져오기
        var rigidbodies = Object.FindObjectsByType<Rigidbody2D>(FindObjectsSortMode.None);

        // 모든 Rigidbody2D의 원래 중력 값을 저장하고 비활성화
        foreach (var rb in rigidbodies)
        {
            originalGravityScales[rb] = rb.gravityScale; // 원래 중력 값 저장
            rb.gravityScale = 0f; // 중력 비활성화
        }
    }

    public void EnableGravity()
    {
        if (!gravityEnabled)
        {
            gravityEnabled = true; // 중력 활성화 상태로 변경

            // 모든 Rigidbody2D의 원래 중력 값을 복원
            foreach (var kvp in originalGravityScales)
            {
                kvp.Key.gravityScale = kvp.Value;
            }

            Debug.Log("Gravity enabled for all objects.");
        }
    }

    // 새로 생성된 오브젝트 등록
    public void RegisterNewObject(GameObject obj)
    {
        var rigidbodies = obj.GetComponentsInChildren<Rigidbody2D>();

        foreach (var rb in rigidbodies)
        {
            RegisterRigidbody(rb);
        }
    }

    // Rigidbody2D를 중력 관리에 등록
    private void RegisterRigidbody(Rigidbody2D rb)
    {
        if (rb == null) return;

        if (!originalGravityScales.ContainsKey(rb))
        {
            originalGravityScales[rb] = rb.gravityScale; // 원래 중력 값 저장
            if (!gravityEnabled)
            {
                rb.gravityScale = 0f; // 중력 비활성화
            }
        }
    }
}