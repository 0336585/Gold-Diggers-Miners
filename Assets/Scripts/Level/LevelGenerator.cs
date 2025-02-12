using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 5; // Y-Axis (vertical)
    [SerializeField] private int columns = 5; // X-Axis (horizontal)
    [SerializeField] private Vector2 startPosition = Vector2.zero; // Starting position of the first level prefab

    [Header("Prefabs")]
    [SerializeField] private GameObject[] levelPrefabs;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Vector2 currentPosition = startPosition;

        for (int row = 0; row < rows; row++)
        {
            Vector2 rowStartPosition = currentPosition;  

            for (int col = 0; col < columns; col++)
            {
                // Randomly select a prefab from the array
                GameObject selectedPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Length)];

                // Instantiate the prefab
                GameObject tile = Instantiate(selectedPrefab, currentPosition, Quaternion.identity);

                // Parent the tile to the generator for organization
                tile.transform.parent = this.transform;

                // Get the prefab's size dynamically
                Bounds tileBounds = GetPrefabBounds(tile);

                // Move to the right by the width of the current prefab
                currentPosition.x += tileBounds.size.x;
            }

            // Reset X position to row start and move downward by row height
            currentPosition = rowStartPosition;
            currentPosition.y -= GetPrefabBounds(levelPrefabs[0]).size.y; // Assuming consistent row height
        }
    }

    private Bounds GetPrefabBounds(GameObject prefab)
    {
        // Create a temporary object to measure bounds
        GameObject temp = Instantiate(prefab);
        Bounds bounds = new Bounds(temp.transform.position, Vector3.zero);

        foreach (Renderer renderer in temp.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        Destroy(temp); // Clean up temporary object
        return bounds;
    }
}