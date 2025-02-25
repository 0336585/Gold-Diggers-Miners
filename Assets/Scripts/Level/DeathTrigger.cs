using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroys any GameObject that enters the trigger zone
        Destroy(collision.gameObject);
    }
}

