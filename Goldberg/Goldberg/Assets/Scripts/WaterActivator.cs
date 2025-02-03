using UnityEngine;

public class ParticleActivator : MonoBehaviour
{
    public ParticleSystem particleSys; // 자식 Particle System
    public float activationAngle = 90f; // 활성화 기준 각도

    private GravityController gravityController;

    void Start()
    {
        // GravityController 참조
        gravityController = FindFirstObjectByType<GravityController>();
        if (gravityController == null)
        {
            Debug.LogError("GravityController not found in the scene!");
        }
    }

    void Update()
    {
        // 중력이 비활성화된 경우 파티클을 정지하고 종료
        if (gravityController == null || !gravityController.gravityEnabled)
        {
            if (particleSys.isPlaying)
            {
                particleSys.Stop(); // 파티클 비활성화
            }
            return;
        }

        // 부모 물체의 Z축 회전 값을 가져오기
        float zRotation = transform.eulerAngles.z;

        // 회전 값을 -180 ~ 180도로 변환
        if (zRotation > 180f)
        {
            zRotation -= 360f;
        }

        // 각도가 활성화 기준 이상인지 확인
        if (Mathf.Abs(zRotation) > activationAngle)
        {
            if (!particleSys.isPlaying)
            {
                particleSys.Play(); // 파티클 활성화
            }
        }
        else
        {
            if (particleSys.isPlaying)
            {
                particleSys.Stop(); // 파티클 비활성화
            }
        }
    }
}
