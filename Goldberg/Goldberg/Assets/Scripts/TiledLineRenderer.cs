using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TiledLineRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    void Awake()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();

        // TextureMode 설정
        lineRenderer.textureMode = LineTextureMode.Tile;
        // 머티리얼의 WrapMode나 Tiling 설정 확인
        // lineRenderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat; // 에디터에서 설정 가능
    }

    void Start() {
        transform.localScale = new Vector3(2, 2, 2); // X, Y, Z 모든 축에 대해 크기를 2배로 조정
        SetWidth(4f, 4f);
    } 

    public void SetWidth(float startWidth, float endWidth)
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, startWidth);
        curve.AddKey(1.0f, endWidth);
        lineRenderer.widthCurve = curve;
    }  

    void Update()
    {
        // 라인 길이 계산
        float length = GetLineLength(lineRenderer);

        // 머티리얼의 Tiling (x = 반복 횟수, y=1)
        lineRenderer.material.mainTextureScale = new Vector2(length, 1f);

        // 필요하다면 Offset(스크롤)
        // float scrollSpeed = 1f;
        // float offset = Time.time * scrollSpeed;
        // lineRenderer.material.mainTextureOffset = new Vector2(offset, 0f);
    }

    float GetLineLength(LineRenderer lr)
    {
        float total = 0f;
        for (int i = 0; i < lr.positionCount - 1; i++)
        {
            total += Vector3.Distance(lr.GetPosition(i), lr.GetPosition(i + 1));
        }
        return total;
    }
}