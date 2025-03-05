using UnityEngine;
using System.Collections;

public class EnemyTargeting : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f; // The detection range
    [SerializeField] private float detectionDelay = 1f; // Delay between checks (in seconds)

    [SerializeField] private BasePlayer targetPlayer; // The current target player
    private BasePlayer[] allPlayers; // Array to store all players in the scene

    private void Start()
    {
        // Find all objects with the BasePlayer script in the scene
        allPlayers = FindObjectsOfType<BasePlayer>();

        // Start the coroutine to check for players with a delay
        StartCoroutine(CheckForPlayerWithDelay());
    }

    // Coroutine to check for players with a delay between checks
    private IEnumerator CheckForPlayerWithDelay()
    {
        while (true)
        {
            CheckForPlayer(); // Check for players within range

            // Wait for the specified delay before checking again
            yield return new WaitForSeconds(detectionDelay);
        }
    }

    // Method to check if a player is within the detection range
    private void CheckForPlayer()
    {
        BasePlayer newTarget = null; // To store the new target if found

        // Iterate through all the players found
        foreach (BasePlayer player in allPlayers)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            // If the player is within the detection range, set them as the target
            if (distance <= detectionRange)
            {
                newTarget = player;
                break; // We only need to track one player, so break after finding the first within range
            }
        }

        // If a player was found within range, set them as the target, otherwise clear the target
        TargetPlayer = newTarget;
    }

    public BasePlayer TargetPlayer
    {
        get { return targetPlayer; }
        private set
        {
            if (targetPlayer != value)
            {
                targetPlayer = value;
            }
        }
    }

    // Gizmos to visualize the detection range in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Color for the detection range
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw a wireframe sphere representing the detection range

        // If a target is assigned, draw a line to it
        if (targetPlayer != null)
        {
            Gizmos.color = Color.green; // Color for the line to the target
            Gizmos.DrawLine(transform.position, targetPlayer.transform.position); // Draw a line to the target
        }
    }
}
