using System.Collections.Generic;
using UnityEngine;

public class SwitchController : WireController
{
    public Sprite[] images; // 전환할 이미지 배열
    public int currentImageIndex = 0;

    private SpriteRenderer spriteRenderer; // 오브젝트의 SpriteRenderer

    protected override void Start()
    {
        // WireController 초기화
        base.Start();

        // SpriteRenderer를 가져옴
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer를 찾을 수 없습니다. 오브젝트에 SpriteRenderer 컴포넌트를 추가하세요.");
            return;
        }

        // 초기 이미지를 설정
        if (images.Length > 0)
        {
            spriteRenderer.sprite = images[0];
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked Switch");
        // 이미지 배열이 비어 있는 경우 동작하지 않음
        if (images.Length == 0 || spriteRenderer == null) return;

        // 클릭 시 이미지 변경
        currentImageIndex = (currentImageIndex + 1) % images.Length;
        spriteRenderer.sprite = images[currentImageIndex];

        // 전류 상태 업데이트
        UpdateElectricityState();
    }

    public override void UpdateElectricityState()
    {
        // 스위치가 꺼져 있으면 전류를 끊음
        if (tag == "Switch" && currentImageIndex != 1)
        {
            electricity = false;
        }
        else
        {
            // 기본 WireController의 UpdateElectricityState 호출
            base.UpdateElectricityState();
        }

        // 연결된 모든 전선에 상태 전파
        foreach (var wire in connectedWires)
        {
            if (wire.electricity != electricity)
            {
                wire.electricity = electricity;
                wire.UpdateElectricityState();
            }
        }
    }
}
