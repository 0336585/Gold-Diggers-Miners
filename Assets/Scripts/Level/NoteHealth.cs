using UnityEngine;

public class NoteHealth : MonoBehaviour
{

    [SerializeField] private int noteHealth = 1;

    public void DamageNote(int _damage)
    {
        noteHealth -= _damage;

        if (noteHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Note Destroyed");
        }
    }
}
