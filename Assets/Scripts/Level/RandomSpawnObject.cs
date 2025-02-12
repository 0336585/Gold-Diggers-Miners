using System.Collections;
using UnityEngine;

public class RandomSpawnObject : MonoBehaviour 
{
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private float[] weights;

    private void Start()
    {
        if (spawnObjects.Length == 0 || weights.Length == 0 || spawnObjects.Length != weights.Length)
        {
            Debug.LogError(gameObject.name + ": spawnObjects and percentages arrays should not be empty and should have the same length.");
            return;
        }
        else
        {
            Instantiate(spawnObjects[GetRandomSpawn()], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        }
    }

    private int GetRandomSpawn()
    {
        float chance = Random.Range(0f, 1f);
        float addNumberBucket = 0;
        float total = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            total += weights[i];
        }

        for (int i = 0; i < spawnObjects.Length; i++)
        {
            if (weights[i] / total + addNumberBucket >= chance)
            {
                return i;
            }
            else
            {
                addNumberBucket += weights[i] / total;
            }
        }
        return 0;
    }
}
