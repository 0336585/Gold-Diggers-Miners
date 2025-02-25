using UnityEngine;

public class NodeHealth : MonoBehaviour
{
    [SerializeField] private float noteHealth = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the ProjectileDamage component
        ProjectileDamage projectileDamage = collision.collider.GetComponent<ProjectileDamage>();

        if (projectileDamage != null)
        {
            // Call the DamageNote method and pass the damage amount
            DamageNode(projectileDamage.GetDamage());
        }
    }

    public void DamageNode(float _damage)
    {
        noteHealth -= _damage;

        if (noteHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Node Destroyed");
        }
    }
}