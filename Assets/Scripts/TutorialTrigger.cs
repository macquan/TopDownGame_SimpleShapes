using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialStep; // The step to activate when triggered

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            // Activate the tutorial step
            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.ShowStep(tutorialStep);
            }

            // Optionally, destroy the trigger after use
            Destroy(gameObject);
        }
    }
}
