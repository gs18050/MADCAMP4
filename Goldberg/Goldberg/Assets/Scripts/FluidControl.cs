using UnityEngine;
using TMPro;

public class FluidControl : MonoBehaviour
{
    public ParticleSystem particleSys; // 제어할 Particle System
    public string targetTag = "Target"; // 충돌 대상 태그
    public TextMeshPro text; // 출력할 텍스트
    public float destroyYThreshold = -20f; // Particle 삭제 기준 Y 좌표
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        // particleSys가 Inspector에서 제대로 설정되었는지 확인
        if (particleSys == null)
        {
            Debug.LogError("Particle System is not assigned!");
            return;
        }

        // Particle 배열 초기화
        particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
    }

    void Update()
    {
        if (particleSys == null || particles == null)
        {
            //Debug.LogError("Particle System or particles array is not initialized!");
            return;
        }

        int aliveParticles = particleSys.GetParticles(particles);

        // Y 좌표 기준으로 Particle 제거
        for (int i = 0; i < aliveParticles; i++)
        {
            if (particles[i].position.y < destroyYThreshold)
            {
                particles[i].remainingLifetime = 0; // 파티클 수명 0으로 설정
            }
        }

        // 업데이트된 Particle 데이터 반영
        particleSys.SetParticles(particles, aliveParticles);
    }

    void OnParticleCollision(GameObject other)
    {
        if (particleSys == null || particles == null)
        {
            Debug.LogError("Particle System or particles array is not initialized!");
            return;
        }
        //Debug.Log("Collision"+other.tag);
        if (other.CompareTag(targetTag) && text != null)
        {
            Debug.Log("Particle collided with target!");

            // 텍스트 업데이트
            text.text = "Start";
            text.fontSize = 8f;
        }

        if (!other.CompareTag("Bucket"))
        {
            // Particle System의 Particle 데이터 가져오기
            int aliveParticles = particleSys.GetParticles(particles);

            for (int i = 0; i < aliveParticles; i++)
            {
                // 충돌한 Particle의 수명을 0으로 설정
                if (Vector3.Distance(particles[i].position, other.transform.position) < 0.1f)
                {
                    particles[i].remainingLifetime = 0; // 파티클 제거
                }
            }

            // 업데이트된 Particle 데이터 반영
            particleSys.SetParticles(particles, aliveParticles);
        }
    }
}