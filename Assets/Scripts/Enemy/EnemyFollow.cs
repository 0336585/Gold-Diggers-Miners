using UnityEngine;
using System.Collections;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;  
    [SerializeField] private float returnDelay = 3f;  

    private EnemyTargeting enemyTargeting;
    private Vector2 originalPosition;
    private bool isReturning = false;

    private void Start()
    {
        enemyTargeting = GetComponent<EnemyTargeting>();  
        originalPosition = transform.position;  
    }

    private void Update()
    {
        if (enemyTargeting.TargetPlayer != null)
        {
            StopAllCoroutines(); // Stop returning if we have a target
            FollowPlayer(enemyTargeting.TargetPlayer);
        }
        else if (!isReturning)
        {
            StartCoroutine(ReturnToOriginalPosition()); // Start return process
        }
    }

    private void FollowPlayer(BasePlayer player)
    {
        // Move toward the player's position
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay); // Wait before returning

        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        isReturning = false;
    }
}
