using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTargeting : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float detectionDelay = 1f;
    [SerializeField] private float lookAroundIntervalMin = 2f;
    [SerializeField] private float lookAroundIntervalMax = 5f;

    private BasePlayer targetPlayer;
    private List<BasePlayer> allPlayers = new List<BasePlayer>(); // Use a list for easier removal

    private void Start()
    {
        // Find all objects with the BasePlayer script in the scene
        allPlayers.AddRange(FindObjectsOfType<BasePlayer>());

        // Start the coroutine to check for players with a delay
        StartCoroutine(CheckForPlayerWithDelay());

        // Start coroutine to make the enemy look around randomly
        StartCoroutine(LookAround());
    }

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
        // Remove any players that have been destroyed
        allPlayers.RemoveAll(player => player == null);

        BasePlayer newTarget = null; // To store the new target if found

        // Iterate through all the players found
        foreach (BasePlayer player in allPlayers)
        {
            // Ensure the player is still valid
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);

                // If the player is within the detection range, set them as the target
                if (distance <= detectionRange)
                {
                    newTarget = player;
                    break; // Stop after finding the first valid player in range
                }
            }
        }

        // Set or clear the target player accordingly
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

    private void Update()
    {
        // Only update if targetPlayer is valid
        if (targetPlayer != null)
        {
            FlipTowardsTarget(); // Continuously update facing direction when targeting a player
        }
    }

    private void FlipTowardsTarget()
    {
        if (targetPlayer == null)
            return; // No target to flip towards

        // Determine the direction to the player
        float directionX = targetPlayer.transform.position.x - transform.position.x;
        float directionY = targetPlayer.transform.position.y - transform.position.y;

        // Define a vertical threshold to prevent flipping when the player is above or below
        float verticalThreshold = 1.0f; // Adjust as needed

        // Flip the enemy sprite based on the horizontal direction
        if (Mathf.Abs(directionY) < verticalThreshold)
        {
            if (directionX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Facing right
            }
            else if (directionX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Facing left
            }
        }
    }

    private IEnumerator LookAround()
    {
        while (true)
        {
            // If no target, flip randomly after a delay
            if (targetPlayer == null)
            {
                // Random delay before flipping
                float delay = Random.Range(lookAroundIntervalMin, lookAroundIntervalMax);
                yield return new WaitForSeconds(delay);

                // Randomly flip the enemy to the left or right
                FlipRandomly();
            }
            else
            {
                // If targeting a player, ensure the enemy faces the player
                FlipTowardsTarget();
            }

            yield return null; // Wait until next frame
        }
    }

    // Flips the enemy randomly to the left or right
    private void FlipRandomly()
    {
        int randomDirection = Random.Range(0, 2); // Randomly pick 0 (left) or 1 (right)

        if (randomDirection == 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing right
        }
    }

    // Gizmos to visualize the detection range in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw a line to the target if it is assigned and valid
        if (targetPlayer != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPlayer.transform.position);
        }
    }
}
