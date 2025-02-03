using UnityEngine;

public class GearController : MonoBehaviour
{
    public float rotationSpeed = 100f; // 기본 회전 속도
    public bool isDriven = false; // 외부 동력 여부

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isDriven)
        {
            // 외부 동력으로 회전 (키보드 입력)
            float input = Input.GetAxis("Horizontal");
            rb.angularVelocity = input * rotationSpeed;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != this.tag) return;
        
        // 충돌한 기어의 Rigidbody 가져오기
        Rigidbody2D otherRb = collision.rigidbody;

        if (otherRb != null)
        {
            // 기어 간의 반대 방향 회전
            float relativeSpeed = rb.angularVelocity;
            otherRb.angularVelocity = -relativeSpeed * (rb.inertia / otherRb.inertia);
        }
    }
}
