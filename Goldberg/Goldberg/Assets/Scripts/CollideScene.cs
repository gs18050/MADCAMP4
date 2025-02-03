using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CollideScene : MonoBehaviour
{
    public string targetSceneName; // Scene to transition to
    public float forceThreshold = 50f; // Minimum force required for transition
    public Image transitionImage; // UI Image component
    public Sprite newSprite; // Image to display before scene transition
    public float delayTime = 1f; // Delay before scene transition

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get Rigidbody2D of the colliding object
        Rigidbody2D otherRigidbody = collision.rigidbody;

        if (otherRigidbody != null)
        {
            // Calculate impact force
            float impactForce = collision.relativeVelocity.magnitude * otherRigidbody.mass;

            Debug.Log($"Impact Force (2D): {impactForce}");

            // Check if the force exceeds the threshold
            if (impactForce >= forceThreshold)
            {
                Debug.Log("Force exceeded threshold in 2D. Switching scene...");
                StartCoroutine(TransitionWithImage());
            }
        }
    }

    private void OnCollisionEnter(Collision collision) // For 3D physics
    {
        Rigidbody otherRigidbody = collision.rigidbody;

        if (otherRigidbody != null)
        {
            float impactForce = collision.relativeVelocity.magnitude * otherRigidbody.mass;

            Debug.Log($"Impact Force (3D): {impactForce}");

            if (impactForce >= forceThreshold)
            {
                Debug.Log("Force exceeded threshold in 3D. Switching scene...");
                StartCoroutine(TransitionWithImage());
            }
        }
    }

    private IEnumerator TransitionWithImage()
    {
        if (transitionImage != null && newSprite != null)
        {
            // Change the image sprite
            transitionImage.sprite = newSprite;
            transitionImage.gameObject.SetActive(true); // Show the image

            // Wait for the specified delay
            yield return new WaitForSeconds(delayTime);

            // Transition to the next scene
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogError("Target scene name is not assigned!");
            }
        }
        else
        {
            Debug.LogError("Transition Image or New Sprite is not assigned!");
        }
    }
}
