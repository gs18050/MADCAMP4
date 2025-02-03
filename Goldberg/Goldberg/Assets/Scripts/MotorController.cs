using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    public Transform receiver; // 전류를 받는 포트
    public bool electricity = false; // 모터의 전류 상태
    public float angularVelocity = 100f; // 모터가 기어에 설정할 각속도 (degrees/sec)
    public string gearTag = "Gear"; // 기어 태그
    public float activationDistance = 0.5f; // 활성화 거리

    private List<Rigidbody2D> nearbyGears = new List<Rigidbody2D>(); // 닿아 있는 기어 추적

    void Update()
    {
        // Receiver가 닿아 있는 Sender 또는 PowerSource 확인
        if (receiver != null)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(receiver.position);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Sender"))
                {
                    WireController connectedWire = hit.GetComponentInParent<WireController>();
                    if (connectedWire != null)
                    {
                        electricity = connectedWire.electricity;
                        break;
                    }
                }
                else if (hit.CompareTag("PowerSource"))
                {
                    PowerSource powerSource = hit.GetComponentInParent<PowerSource>();
                    if (powerSource != null && powerSource.sender == hit.transform)
                    {
                        electricity = powerSource.electricity;
                        break;
                    }
                }
            }
        }

        // 전류가 흐르면 닿아 있는 모든 기어에 각속도 적용
        if (electricity)
        {
            foreach (var gear in nearbyGears)
            {
                if (gear != null)
                {
                    float distance = Vector2.Distance(transform.position, gear.transform.position);
                    if (distance <= activationDistance)
                    {
                        // 각속도 설정
                        gear.angularVelocity = angularVelocity;
                    }
                }
            }
        }
        else
        {
            // 전류가 없으면 각속도 초기화
            foreach (var gear in nearbyGears)
            {
                if (gear != null)
                {
                    gear.angularVelocity = 0f;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(gearTag))
        {
            Rigidbody2D gearRigidbody = collision.GetComponent<Rigidbody2D>();
            if (gearRigidbody != null && !nearbyGears.Contains(gearRigidbody))
            {
                nearbyGears.Add(gearRigidbody);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(gearTag))
        {
            Rigidbody2D gearRigidbody = collision.GetComponent<Rigidbody2D>();
            if (gearRigidbody != null && nearbyGears.Contains(gearRigidbody))
            {
                nearbyGears.Remove(gearRigidbody);
                // 기어가 더 이상 모터와 연결되지 않으면 각속도 초기화
                gearRigidbody.angularVelocity = 0f;
            }
        }
    }
}
