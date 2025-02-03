using UnityEngine;
using TMPro;

public class TextResizer : MonoBehaviour
{
    public TextMeshPro textMesh; // TextMeshPro 컴포넌트
    public Transform buttonTransform; // StartButton Transform

    void Update()
    {
        if (textMesh != null && buttonTransform != null)
        {
            // 텍스트 위치를 버튼 중심에 맞춤
            textMesh.transform.position = buttonTransform.position;

            // 텍스트 크기를 버튼 크기에 맞게 조정
            float buttonWidth = buttonTransform.localScale.x;
            float buttonHeight = buttonTransform.localScale.y;
            textMesh.fontSize = Mathf.Min(buttonWidth, buttonHeight) * 8; // 폰트 크기 비율 조정
        }
    }
}
