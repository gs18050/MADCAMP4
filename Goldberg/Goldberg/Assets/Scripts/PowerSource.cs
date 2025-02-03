using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public bool electricity = false; // 전류 상태
    public bool isActive = true;     // 전원 활성 상태 (발전기 조건 추가 가능)
    public Transform sender;         // 전류를 보내는 포트
    private GravityController gravityController;
    public string powerType = "Battery";

    void Start() {
        gravityController = FindFirstObjectByType<GravityController>();
    }

    void Update()
    {
        if (!gravityController.gravityEnabled) {
            electricity = false;
            return;
        }
        if (powerType == "Battery") {
            electricity = true;
            return;
        }
        // 특정 조건 하에서 전원을 제공 (예: 발전기 상태 업데이트)
        electricity = isActive; // 단순히 활성 상태에 따라 전류 제공
    }
}
