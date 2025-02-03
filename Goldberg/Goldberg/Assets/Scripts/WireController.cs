using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    public Transform sender;    // 전류를 보내는 포트
    public Transform receiver;  // 전류를 받는 포트
    public bool electricity = false;  // 전선의 현재 전류 상태

    protected List<WireController> connectedWires = new List<WireController>(); // 연결된 전선 리스트

    protected virtual void Start()
    {
        InitializeConnections(); // 초기 연결 상태 설정
        UpdateElectricityState();
    }

    public virtual void Update()
    {
        UpdateElectricityState(); // 매 프레임 전류 상태 확인
    }

    public virtual void InitializeConnections()
    {
        if (receiver != null)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(receiver.position);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Sender"))
                {
                    WireController wire = hit.GetComponentInParent<WireController>();
                    if (wire != null && !connectedWires.Contains(wire))
                    {
                        connectedWires.Add(wire);
                    }
                }
                else if (hit.CompareTag("PowerSource"))
                {
                    PowerSource powerSource = hit.GetComponentInParent<PowerSource>();
                    if (powerSource != null && powerSource.sender == hit.transform)
                    {
                        electricity = powerSource.electricity;
                    }
                }
            }
        }
    }

    public virtual void UpdateElectricityState()
    {
        bool newElectricityState = false;

        // Receiver가 연결된 Sender에서 전류 상태 확인
        if (receiver != null)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(receiver.position);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Sender"))
                {
                    WireController wire = hit.GetComponentInParent<WireController>();
                    if (wire != null)
                    {
                        newElectricityState = wire.electricity;
                        break;
                    }
                }
                else if (hit.CompareTag("PowerSource"))
                {
                    PowerSource powerSource = hit.GetComponentInParent<PowerSource>();
                    if (powerSource != null && powerSource.sender == hit.transform)
                    {
                        newElectricityState = powerSource.electricity;
                        break;
                    }
                }
            }
        }

        // 전류 상태가 변경된 경우만 업데이트
        if (newElectricityState != electricity)
        {
            electricity = newElectricityState;
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

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sender") || collision.CompareTag("PowerSource"))
        {
            WireController wire = collision.GetComponentInParent<WireController>();
            if (wire != null && !connectedWires.Contains(wire))
            {
                connectedWires.Add(wire);
                UpdateElectricityState();
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        WireController wire = collision.GetComponentInParent<WireController>();
        if (wire != null && connectedWires.Contains(wire))
        {
            connectedWires.Remove(wire);
            UpdateElectricityState();
        }
    }
}
