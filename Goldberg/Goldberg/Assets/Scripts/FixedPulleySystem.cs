using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FixedPulleySystem : MonoBehaviour
{
    public Transform pulleyAnchor;  // 도르래의 고정 지점
    public Transform weight;        // 무게추 Transform
    public Transform startButton;   // Start 버튼 Transform (밧줄 시작점의 부모)
    public Transform weightAnchor;

    private LineRenderer lineRenderer;
    private Rigidbody2D weightRigidbody;
    private Rigidbody2D startButtonRigidbody;
    private float textureTilingScale = 1f;

    public float tensionMultiplier = 0.1f; // 장력 계수
    private GravityController gravityController; // 중력 상태 확인

    void Start()
    {
        // LineRenderer 초기화
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3; // 고정점, 도르래 접점, 무게추
        lineRenderer.useWorldSpace = true;

        // Rigidbody 초기화
        weightRigidbody = weight.GetComponent<Rigidbody2D>();
        startButtonRigidbody = startButton.GetComponent<Rigidbody2D>();

        if (weightRigidbody == null || startButtonRigidbody == null)
        {
            Debug.LogError("Both weight and startButton must have Rigidbody2D components.");
        }

        // GravityController 참조
        gravityController = FindFirstObjectByType<GravityController>();
        if (gravityController == null)
        {
            Debug.LogError("GravityController not found in the scene!");
        }
    }

    void Update()
    {
        if (pulleyAnchor == null || weightAnchor == null || startButton == null)
        {
            Debug.LogError("PulleyAnchor, WeightAnchor, or StartButton is not assigned!");
            return;
        }

        // 이름으로 자식 오브젝트 찾기
        Transform ropeStartAnchor = startButton.Find("Anchor");
        if (ropeStartAnchor == null)
        {
            Debug.LogError("Anchor child not found under Start Button!");
            return;
        }

        // (1) 월드 좌표 계산
        Vector3 worldPointPulley = pulleyAnchor.position;
        Vector3 worldPointStart = ropeStartAnchor.position;
        Vector3 worldPointWeight = weight.position;

        // (2) LineRenderer에 좌표 설정
        lineRenderer.SetPosition(0, worldPointStart);
        lineRenderer.SetPosition(1, worldPointPulley);
        lineRenderer.SetPosition(2, worldPointWeight);

        // (3) 텍스처 타일링
        float lineLength = Vector3.Distance(worldPointStart, worldPointPulley) + Vector3.Distance(worldPointPulley, worldPointWeight);
        if (lineRenderer.material)
        {
            lineRenderer.material.mainTextureScale = new Vector2(lineLength * textureTilingScale, 1f);
        }

        // 중력이 활성화된 경우에만 동작
        if (gravityController == null || !gravityController.gravityEnabled)
        {
            return;
        }

        // (4) 장력 계산 및 적용
        float tension = CalculateTension();
        ApplyTension(tension);
    }

    private float CalculateTension()
    {
        // 기본 장력: 무게추의 질량 * 중력 가속도 * 계수
        return weightRigidbody.mass * Mathf.Abs(Physics2D.gravity.y) * tensionMultiplier;
    }

    private void ApplyTension(float tension)
    {
        // 무게추에 작용하는 상향 장력
        Vector2 forceOnWeight = Vector2.up * tension;
        weightRigidbody.AddForce(forceOnWeight);

        // Start 버튼에 작용하는 하향 장력
        Vector2 forceOnStartButton = -Vector2.up * tension;
        startButtonRigidbody.AddTorque(-forceOnStartButton.magnitude);

        //Debug.Log($"Tension: {tension}, Force on Weight: {forceOnWeight}, Torque on Start Button: {forceOnStartButton.magnitude}");
    }
}
