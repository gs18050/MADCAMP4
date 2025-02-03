using System;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    private GameObject selectedPrefabInstance; // 드래그 중인 Prefab의 인스턴스
    private Camera mainCamera;

    public string ghostLayerName = "Ghost"; // 미리보기 Layer 이름

    public float rotationSpeed = 100f; // 회전 속도
    public float scaleSpeed = 0.1f; // 크기 조정 속도

    private Dictionary<Rigidbody2D, float> originalMassMap = new Dictionary<Rigidbody2D, float>(); // 원래 질량 저장
    private Dictionary<GameObject, int> originalLayerMap = new Dictionary<GameObject, int>(); // 원래 Layer 저장
    private Dictionary<GameObject, string> originalTagMap = new Dictionary<GameObject, string>(); // 원래 태그 저장

    private GravityController gravityController;

    void Start()
    {
        mainCamera = Camera.main;

        // GravityController 참조
        gravityController = FindFirstObjectByType<GravityController>();
    }

    void Update()
    {
        // 오브젝트 선택 또는 드래그 시작
        if (Input.GetMouseButtonDown(0) && gravityController != null && !gravityController.gravityEnabled)
        {
            Debug.Log("Clicked");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = LayerMask.GetMask("Default", "Bucket", "Gear"); // Default 및 Bucket Layer 포함
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);
            //RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            //Debug.Log(hit.collider.gameObject.name);

            if (hit.collider != null)
            {
                // 이미 배치된 오브젝트 선택
                selectedPrefabInstance = hit.collider.gameObject;
                Debug.Log($"Selected Existing Object: {selectedPrefabInstance.name}");

                // 원래 Layer 저장 및 Ghost Layer로 변경
                CacheOriginalLayer(selectedPrefabInstance);
                SetLayerRecursively(selectedPrefabInstance, LayerMask.NameToLayer(ghostLayerName));

                // 원래 태그 저장
                CacheOriginalTag(selectedPrefabInstance);
            }
            else
            {
                // 새로운 Prefab 배치
                GameObject prefab = FindFirstObjectByType<PrefabSelector>()?.GetAndClearSelectedPrefab();

                if (prefab != null)
                {
                    selectedPrefabInstance = Instantiate(prefab, GetMouseWorldPosition(), Quaternion.identity);
                    CacheOriginalLayer(selectedPrefabInstance);
                    SetLayerRecursively(selectedPrefabInstance, LayerMask.NameToLayer(ghostLayerName));

                    // 선택된 Prefab과 자식들의 원래 질량 저장
                    CacheOriginalMass(selectedPrefabInstance);

                    // 원래 태그 저장
                    CacheOriginalTag(selectedPrefabInstance);

                    gravityController?.RegisterNewObject(selectedPrefabInstance);
                }
            }
        }

        // 드래그 중
        if (Input.GetMouseButton(0) && selectedPrefabInstance != null)
        {
            selectedPrefabInstance.transform.position = GetMouseWorldPosition();
            HandleRotation();
            HandleScaling();
        }

        // 드래그 종료 (Drop)
        if (Input.GetMouseButtonUp(0) && selectedPrefabInstance != null)
        {
            // 드래그가 종료되면 Layer를 원래 Layer로 복원
            RestoreOriginalLayer(selectedPrefabInstance);

            // 원래 태그 복원
            RestoreOriginalTag(selectedPrefabInstance);

            selectedPrefabInstance = null; // 선택 해제
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            selectedPrefabInstance.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            selectedPrefabInstance.transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleScaling()
    {
        float scaleChange = 0f;

        if (Input.GetKeyDown(KeyCode.W))
        {
            scaleChange = scaleSpeed; // W 키로 크기 증가
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            scaleChange = -scaleSpeed; // S 키로 크기 감소
        }

        if (scaleChange != 0)
        {
            // 현재 크기를 기준으로 크기 비율 계산
            Vector3 currentScale = selectedPrefabInstance.transform.localScale;
            Vector3 newScale = currentScale + Vector3.one * scaleChange;

            // 최소/최대 크기 제한
            newScale = Vector3.Max(newScale, Vector3.one * 0.1f); // 최소 크기
            newScale = Vector3.Min(newScale, Vector3.one * 10f); // 최대 크기
            selectedPrefabInstance.transform.localScale = newScale;

            // 크기 비율 계산
            float scaleFactor = newScale.x * newScale.y;

            // 모든 Rigidbody2D의 질량 조정
            AdjustMassRecursively(selectedPrefabInstance, scaleFactor);
        }
    }

    private void CacheOriginalMass(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null && !originalMassMap.ContainsKey(rb))
        {
            originalMassMap[rb] = rb.mass; // 원래 질량 저장
        }

        foreach (Transform child in obj.transform)
        {
            CacheOriginalMass(child.gameObject);
        }
    }

    private void AdjustMassRecursively(GameObject obj, float scaleFactor)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null && originalMassMap.ContainsKey(rb))
        {
            rb.mass = originalMassMap[rb] * scaleFactor; // 면적 비례
        }

        foreach (Transform child in obj.transform)
        {
            AdjustMassRecursively(child.gameObject, scaleFactor);
        }
    }

    private void CacheOriginalLayer(GameObject obj)
    {
        if (!originalLayerMap.ContainsKey(obj))
        {
            originalLayerMap[obj] = obj.layer; // 원래 Layer 저장
        }

        foreach (Transform child in obj.transform)
        {
            CacheOriginalLayer(child.gameObject);
        }
    }

    private void RestoreOriginalLayer(GameObject obj)
    {
        if (originalLayerMap.ContainsKey(obj))
        {
            obj.layer = originalLayerMap[obj]; // Layer 복원
        }

        foreach (Transform child in obj.transform)
        {
            RestoreOriginalLayer(child.gameObject);
        }
    }

    private void CacheOriginalTag(GameObject obj)
    {
        if (!originalTagMap.ContainsKey(obj))
        {
            originalTagMap[obj] = obj.tag; // 원래 태그 저장
        }

        foreach (Transform child in obj.transform)
        {
            CacheOriginalTag(child.gameObject);
        }
    }

    private void RestoreOriginalTag(GameObject obj)
    {
        if (originalTagMap.ContainsKey(obj))
        {
            obj.tag = originalTagMap[obj]; // 태그 복원
        }

        foreach (Transform child in obj.transform)
        {
            RestoreOriginalTag(child.gameObject);
        }
    }
}
