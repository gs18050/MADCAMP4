using UnityEngine;
using TMPro;

public class UserTyping : MonoBehaviour
{
    public TMP_InputField inputField; // Input Field 컴포넌트
    public GravityController gravityController; // GravityController 참조

    void Start()
    {
        // Input Field의 onValueChanged 이벤트에 리스너 추가
        inputField.onValueChanged.AddListener(OnTextInput);
    }

    void OnTextInput(string newText)
    {
        // 한 글자라도 입력이 있으면 중력을 활성화
        if (!string.IsNullOrEmpty(newText) && gravityController != null)
        {
            gravityController.EnableGravity();
        }
    }
}
