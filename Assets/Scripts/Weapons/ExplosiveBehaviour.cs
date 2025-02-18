using UnityEngine;

public class ExplosiveBehaviour : MonoBehaviour
{
    [SerializeField] private bool hasDetonationTime = true;
    [SerializeField] private float detonationTime = 2f;

    private void Update()
    {
        if(hasDetonationTime)
        {
            Destroy(gameObject, detonationTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
