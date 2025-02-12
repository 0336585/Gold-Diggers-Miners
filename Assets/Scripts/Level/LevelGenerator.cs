using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 5; // Y-Axis (vertical)
    [SerializeField] private int columns = 5; // X-Axis (horizontal)
    [SerializeField] private float distance = 1f; // Distance between prefabs
    [SerializeField] private Vector2 startPosition = Vector2.zero; // Starting position of the first level prefab

    [Header("Prefabs")]
    [SerializeField] private GameObject[] levelPrefabs;  

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
                Vector3 position = new Vector3(
                    startPosition.x + col * distance, // X position
                    startPosition.y - row * distance, // Y position (negative to go downward)
                    0
                );

                // Instantiate the prefab at the calculated position
                GameObject tile = Instantiate(selectedPrefab, position, Quaternion.identity);

                // Parent the tile to the generator for organization
                tile.transform.parent = this.transform;
            }
        }
    }
}