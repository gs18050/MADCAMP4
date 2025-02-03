using UnityEngine;
using UnityEngine.UI;

public class lightSwitch : MonoBehaviour
{
    public Sprite[] images; // 전환할 이미지 배열
    private int currentImageIndex = 0;

    public Image buttonImage; // 버튼의 Image 컴포넌트를 Inspector에서 직접 연결

    void Start()
    {
        // buttonImage가 설정되지 않은 경우 경고 메시지 출력
        if (buttonImage == null)
        {
            Debug.LogError("buttonImage가 설정되지 않았습니다. Inspector에서 버튼의 Image 컴포넌트를 연결하세요.");
            return;
        }

        if (images.Length > 0)
        {
            buttonImage.sprite = images[0];
        }
    }

    public void SwitchImage()
    {
        // 이미지 배열이 비어 있거나 buttonImage가 null인 경우 동작하지 않음
        if (images.Length == 0 || buttonImage == null) return;

        currentImageIndex = (currentImageIndex + 1) % images.Length;
        buttonImage.sprite = images[currentImageIndex];
    }
}
