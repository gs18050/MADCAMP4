using UnityEngine;
using UnityEngine.EventSystems; // Event systems 사용
using UnityEngine.UI; // UI 컴포넌트 사용

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject itemPrefab; // 드롭할 때 생성할 객체의 프리팹
    private GameObject tempObject; // 드래그하는 동안 보여줄 임시 객체

    public void OnBeginDrag(PointerEventData eventData)
    {
        tempObject = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform.parent);
        tempObject.GetComponent<Image>().raycastTarget = false; // 드래그 중 raycast 방지
    }

    public void OnDrag(PointerEventData eventData)
    {
        tempObject.transform.position = Input.mousePosition; // 마우스 위치로 이동
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("GameArea")) // 드롭 영역 확인
        {
            Instantiate(itemPrefab, hit.point, Quaternion.identity); // 실제 객체 생성
        }
        Destroy(tempObject); // 임시 객체 삭제
    }
}