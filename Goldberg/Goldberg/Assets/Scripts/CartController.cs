using UnityEngine;

public class CartController : MonoBehaviour
{
    public GameObject objectOnCart; // 위에 있는 물체
    private bool flag = false;
    public Vector2 launchDirection = new Vector2(1, 1); // 발사 방향 (오른쪽 위)
    public float launchForce = 10f; // 발사 강도

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트의 Tag가 "Trigger"일 경우
        if (collision.collider.CompareTag("Trigger")&&!flag)
        {
            Debug.Log("TriggerObject와 충돌! ObjectOnCart 분리.");
            flag = true;
            DetachObject();
        }
    }

    void DetachObject()
    {
        if (objectOnCart != null)
        {
            // 부모 관계 해제
            objectOnCart.transform.parent = null;

            // Rigidbody 2D를 Dynamic으로 변경
            Rigidbody2D rb = objectOnCart.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1; // 중력 활성화

                rb.AddForce(launchDirection.normalized * launchForce, ForceMode2D.Impulse);
            }
        }
    }
}
