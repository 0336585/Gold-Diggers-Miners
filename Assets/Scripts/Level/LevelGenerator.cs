using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 5; // Y-Axis (vertical)
    [SerializeField] private int columns = 5; // X-Axis (horizontal)
    [SerializeField] private float distance = 1f; // Distance between prefabs

    [Header("Prefabs")]
    [SerializeField] private GameObject[] levelPrefabs; // Array of level prefabs

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Randomly select a prefab from the array
                GameObject selectedPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Length)];

                // Calculate position for the prefab (X and Y axes only)
                Vector3 position = new Vector3(col * distance, row * distance, 0);

                // Instantiate the prefab at the calculated position
                GameObject tile = Instantiate(selectedPrefab, position, Quaternion.identity);

                // Parent the tile to the generator for organization
                tile.transform.parent = this.transform;
            }
        }
    }
}