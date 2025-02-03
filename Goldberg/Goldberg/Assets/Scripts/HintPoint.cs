using System.Collections.Generic;
using UnityEngine;

public class HintPoint : MonoBehaviour
{
    public List<HingeJoint2D> connectedHinges = new List<HingeJoint2D>(); // 연결된 HingeJoint2D 목록

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TrackEnd")) // TrackEnd와 충돌 감지
        {
            // TrackEnd의 부모 오브젝트 가져오기
            GameObject track = collision.transform.parent.gameObject;

            // 부모 오브젝트의 Rigidbody2D 가져오기
            Rigidbody2D trackRigidbody = track.GetComponent<Rigidbody2D>();
            if (trackRigidbody != null)
            {
                // HintPoint에 HingeJoint2D 추가
                var hinge = track.AddComponent<HingeJoint2D>();
                hinge.connectedBody = GetComponent<Rigidbody2D>(); // HintPoint에 연결

                // Anchor 설정
                //hinge.autoConfigureConnectedAnchor = false;
                hinge.anchor = track.transform.InverseTransformPoint(collision.transform.position); // TrackEnd의 로컬 좌표
                //hinge.connectedAnchor = transform.position; // HintPoint의 월드 좌표

                // 연결된 HingeJoint2D 목록에 추가
                connectedHinges.Add(hinge);

                Debug.Log($"Connected: {track.name}");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TrackEnd")) // 연결 해제 조건
        {
            // 트랙과 HintPoint 간 거리 계산
            float distance = Vector2.Distance(transform.position, collision.transform.position);

            // 일정 거리 이상일 때만 힌지 제거
            if (distance > 1.0f) // 거리 기준, 필요에 따라 조정
            {
                // TrackEnd의 부모 오브젝트 가져오기
                GameObject track = collision.transform.parent.gameObject;

                // 해당 트랙과 연결된 HingeJoint2D 찾기
                HingeJoint2D hingeToRemove = connectedHinges.Find(hinge => hinge.connectedBody == track.GetComponent<Rigidbody2D>());

                if (hingeToRemove != null)
                {
                    // HingeJoint2D 제거
                    connectedHinges.Remove(hingeToRemove);
                    Destroy(hingeToRemove);

                    Debug.Log($"Disconnected: {track.name}");
                }
            }
        }
    }
}
