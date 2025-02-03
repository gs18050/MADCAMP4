using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FanWindArea : MonoBehaviour
{
    [Tooltip("바람이 미치는 힘의 세기")]
    public float windForce = 50f;

    private Vector2 windDirection; // 팬의 로컬 방향
    private GravityController gravityController;
    private Transform receiver; // 부모의 receiver 포트
    private bool electricity = false;

    void Start()
    {
        gravityController = FindFirstObjectByType<GravityController>();
        if (gravityController == null)
        {
            Debug.LogError("GravityController not found in the scene!");
        }

        // 부모 오브젝트에서 receiver를 찾음
        Transform parent = transform.parent;
        if (parent != null)
        {
            receiver = parent.Find("receiver");
            if (receiver == null)
            {
                Debug.LogError("Receiver not found in the parent object!");
            }
        }
        else
        {
            Debug.LogError("FanWindArea must have a parent object with a receiver!");
        }

        // 초기 windDirection 설정
        windDirection = Vector2.right; // 팬의 로컬 +X 방향
    }

    private void Update()
    {
        // 팬의 로컬 X축 방향을 월드 좌표로 변환
        windDirection = transform.TransformDirection(Vector2.right);

        // Receiver가 연결된 Sender에서 전력 상태 확인
        if (receiver != null)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(receiver.position);
            electricity = false; // 초기화
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Sender"))
                {
                    WireController connectedWire = hit.GetComponentInParent<WireController>();
                    if (connectedWire != null)
                    {
                        electricity = connectedWire.electricity;
                        break;
                    }
                }
                else if (hit.CompareTag("PowerSource"))
                {
                    PowerSource powerSource = hit.GetComponentInParent<PowerSource>();
                    if (powerSource != null && powerSource.sender == hit.transform)
                    {
                        electricity = powerSource.electricity;
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 중력이 활성화되고 팬이 전력을 받을 때만 동작
        if (gravityController != null && gravityController.gravityEnabled && electricity)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 팬의 로컬 좌표계 기준 +X 방향으로 바람 적용
                Vector2 forceApplied = windDirection.normalized * windForce;
                rb.AddForce(forceApplied, ForceMode2D.Force);
                //Debug.Log($"Fan local direction: {Vector2.right}, Fan world direction: {transform.right}, Wind direction: {windDirection}");
                //Debug.Log($"Applying wind force: {forceApplied} to {other.name}");
            }
            else
            {
                //Debug.Log($"No Rigidbody2D found on {other.name}");
            }
        }
        else
        {
            //Debug.Log("Fan is not active (either no electricity or gravity is disabled).");
        }
    }
}
