using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PulleySystem : MonoBehaviour
{
    public Transform pulleyAnchor; // 도르래 연결 지점 (자식 오브젝트)
    public Transform weightAnchor; // 무게추 연결 지점 (자식 오브젝트)
    public Transform pulley;
    public Transform weight;       // 무게추 Transform

    private LineRenderer lineRenderer;

    [SerializeField]
    private float textureTilingScale = 1f; // 텍스처 타일링 스케일

    void Start()
    {
        // LineRenderer 초기화
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        // LineRenderer가 월드 좌표를 사용하도록 설정
        lineRenderer.useWorldSpace = true;

        // LineRenderer 초기 설정
        SetupLineRenderer();
        SetWidth(4f, 4f);
    }

    public void SetWidth(float startWidth, float endWidth)
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, startWidth);
        curve.AddKey(1.0f, endWidth);
        lineRenderer.widthCurve = curve;
    }

    void SetupLineRenderer()
    {
        // 라인의 텍스처를 타일 모드로 설정
        lineRenderer.textureMode = LineTextureMode.Tile;
    }

    void Update()
    {
        if (pulleyAnchor == null || weightAnchor == null)
        {
            Debug.LogError("PulleyAnchor or WeightAnchor is not assigned!");
            return;
        }

        // (1) 부모의 로컬 좌표계를 기준으로 자식 오브젝트의 월드 좌표 계산
        Vector3 worldPointPulley = pulleyAnchor.position;
        Vector3 worldPointWeight = weightAnchor.position;

        // 디버그 출력
        //Debug.Log($"Pulley: {worldPointPulley}, Weight: {worldPointWeight}");

        // (2) LineRenderer에 좌표 설정
        lineRenderer.SetPosition(0, worldPointPulley);
        lineRenderer.SetPosition(1, worldPointWeight);

        // (3) 라인 길이에 따른 텍스처 타일링 조정
        float lineLength = Vector3.Distance(worldPointPulley, worldPointWeight);
        if (lineRenderer.material)
        {
            lineRenderer.material.mainTextureScale = new Vector2(lineLength * textureTilingScale, 1f);
        }
    }
}
