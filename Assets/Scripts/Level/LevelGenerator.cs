using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 5; // Y-Axis (vertical)
    [SerializeField] private int columns = 5; // X-Axis (horizontal)
    [SerializeField] private Vector2 startPosition = Vector2.zero; // Starting position of the first level prefab

    [Header("Prefabs")]
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private GameObject introPrefab;
    [SerializeField] private GameObject deathTriggerPrefab;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Vector2 currentPosition = startPosition;
        float totalWidth = 0f;
        float totalHeight = 0f;

        // Spawn the intro prefab to the left of the first tile
        if (introPrefab != null)
        {
            GameObject introTile = Instantiate(introPrefab, currentPosition, Quaternion.identity);
            introTile.transform.parent = this.transform;

            // Get intro prefab size and adjust start position
            Bounds introBounds = GetPrefabBounds(introTile);
            currentPosition.x += introBounds.size.x; // Shift to the right
        }

        float levelStartX = currentPosition.x; // Store the leftmost X position

        for (int row = 0; row < rows; row++)
        {
            Vector2 rowStartPosition = currentPosition;
            float rowWidth = 0f;

            for (int col = 0; col < columns; col++)
            {
                // Randomly select a prefab from the array
                GameObject selectedPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Length)];
                // Instantiate the prefab
                GameObject tile = Instantiate(selectedPrefab, currentPosition, Quaternion.identity);
                tile.transform.parent = this.transform;

                // Get prefab size dynamically
                Bounds tileBounds = GetPrefabBounds(tile);
                // Move to the right by the width of the current prefab
                rowWidth += tileBounds.size.x;
                currentPosition.x += tileBounds.size.x;
            }

            // Reset X position to row start and move downward by row height
            totalWidth = Mathf.Max(totalWidth, rowWidth);
            currentPosition = rowStartPosition;
            currentPosition.y -= GetPrefabBounds(levelPrefabs[0]).size.y; // Assuming consistent row height
        }

        totalHeight = Mathf.Abs(currentPosition.y - startPosition.y);
        SpawnDeathTrigger(totalWidth * 2, currentPosition.y, levelStartX); // Death trigger is 2x the size of the level
    }

    private void SpawnDeathTrigger(float width, float yPos, float levelStartX)
    {
        if (deathTriggerPrefab != null)
        {
            Vector2 spawnPosition = new Vector2(levelStartX + width / 2, yPos - 1f); // Ensure it aligns with the level start
            GameObject deathTrigger = Instantiate(deathTriggerPrefab, spawnPosition, Quaternion.identity);
            deathTrigger.transform.localScale = new Vector3(width * 4, 1f, 1f); // Adjust width to be 4x the level width
        }
    }

    private Bounds GetPrefabBounds(GameObject prefab)
    {
        GameObject temp = Instantiate(prefab);
        Bounds bounds = new Bounds(temp.transform.position, Vector3.zero);

        foreach (Renderer renderer in temp.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        Destroy(temp);
        return bounds;
    }
}
