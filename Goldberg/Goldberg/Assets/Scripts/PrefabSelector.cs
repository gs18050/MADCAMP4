using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하기 위한 네임스페이스

public class PrefabSelector : MonoBehaviour
{
    public GameObject[] prefabs; // 선택 가능한 Prefab 리스트
    public string[] prefabNames; // 각 Prefab에 대한 텍스트 리스트
    public Button[] buttons; // UI 버튼 리스트

    public int CurrentInd = 0;
    private GameObject selectedPrefab; // 현재 선택된 Prefab
    private GravityController gravityController;

    void Start()
    {
        gravityController = FindFirstObjectByType<GravityController>();

        // 버튼 텍스트 업데이트
        UpdateButtonTexts();
    }

    // 버튼 클릭 시 호출되는 함수
    public void SelectPrefab(int buttonIndex)
    {
        if (!gravityController.gravityEnabled)
        {
            int prefabIndex = ((CurrentInd + buttonIndex) % prefabs.Length + prefabs.Length) % prefabs.Length;
            selectedPrefab = prefabs[prefabIndex];
            Debug.Log($"Selected Prefab: {prefabs[prefabIndex].name}");
        }
    }

    // 인덱스를 변경하고 버튼 텍스트를 업데이트
    public void ChangeInd(int diff)
    {
        CurrentInd += diff;
        UpdateButtonTexts();
    }

    // 선택된 Prefab 가져오기 (배치 후 초기화)
    public GameObject GetAndClearSelectedPrefab()
    {
        GameObject prefab = selectedPrefab;
        selectedPrefab = null; // 배치 후 선택 초기화
        return prefab;
    }

    // 선택된 Prefab이 있는지 확인
    public bool HasSelectedPrefab()
    {
        return selectedPrefab != null;
    }

    // 버튼 텍스트를 업데이트
    private void UpdateButtonTexts()
    {
        if (prefabs.Length != prefabNames.Length)
        {
            Debug.LogError("Prefabs and prefabNames arrays must have the same length!");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int prefabIndex = ((CurrentInd + i) % prefabs.Length + prefabs.Length) % prefabs.Length;

            // 버튼 자식의 TextMeshPro 컴포넌트를 가져와 텍스트 설정
            TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = prefabNames[prefabIndex];
            }
            else
            {
                Debug.LogError($"Button at index {i} does not have a TextMeshProUGUI component.");
            }

            // 버튼 클릭 이벤트 연결
            int capturedIndex = i; // Closure 문제 방지
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => SelectPrefab(capturedIndex));
        }
    }
}
