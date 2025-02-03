using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Vector3 pulleyCenter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pulleyCenter = transform.position;
        Debug.Log("pulleyCenter111: " + pulleyCenter);

        
    }

    // Update is called once per frame
    void Update()
    {
        pulleyCenter = transform.position;
        Debug.Log("pulleyCenter111: " + pulleyCenter);
        
    }
}
