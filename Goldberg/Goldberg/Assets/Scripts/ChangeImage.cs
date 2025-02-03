using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Image targetImage; // 변경할 UI Image
    public Sprite newSprite; // 3초 뒤에 변경될 이미지
    public float delay = 3f; // 이미지 변경 전 대기 시간 (초)

    void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("Target Image is not assigned!");
            return;
        }

        if (newSprite == null)
        {
            Debug.LogError("New Sprite is not assigned!");
            return;
        }

        // 3초 후에 ChangeImage 메서드 호출
        Invoke("ChangeImage", delay);
    }

    void ChangeImage()
    {
        targetImage.sprite = newSprite; // 이미지 변경
        Debug.Log("Image has been changed!");
    }
}
