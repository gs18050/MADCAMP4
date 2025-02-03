using UnityEngine;
using TMPro;

public class DynamicTextUpdater : MonoBehaviour
{
    public TextMeshPro text;

    void Start()
    {
        UpdateText("");
    }

    public void UpdateText(string newText)
    {
        if (text != null)
        {
            text.text = newText;
        }
    }
}