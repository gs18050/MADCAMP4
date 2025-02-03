using UnityEngine;

public class RockController : MonoBehaviour
{
    [Tooltip("Rock이 사라질 때 적용되는 무게")]
    public float newMass = 8f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "catch" 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Catch"))
        {
            Rigidbody2D catchRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (catchRigidbody != null)
            {
                // Rigidbody 설정 변경
                catchRigidbody.isKinematic = false; // Kinematic에서 Dynamic으로 전환
                catchRigidbody.mass = newMass;       // 무게를 8로 설정

                Debug.Log($"Changed object '{collision.gameObject.name}' to dynamic with mass {newMass}.");
            }
            else
            {
                Debug.LogWarning("Collision object with 'catch' tag has no Rigidbody2D component!");
            }

            // Rock 오브젝트 제거
            Destroy(gameObject);
        }
    }
}
