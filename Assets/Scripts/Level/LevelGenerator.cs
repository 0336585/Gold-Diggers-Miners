using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 5; // Y-Axis (vertical)
    [SerializeField] private int columns = 5; // X-Axis (horizontal)
    [SerializeField] private Vector2 startPosition = Vector2.zero; // Center position of the grid

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
        // Determine a reference tile size (using the first level prefab as reference)
        float tileWidth = 1f;
        float tileHeight = 1f;
        if (levelPrefabs.Length > 0)
        {
            tileWidth = GetPrefabBounds(levelPrefabs[0]).size.x;
            tileHeight = GetPrefabBounds(levelPrefabs[0]).size.y;
        }

        // Calculate total row width and adjust starting x so that the row is centered
        float totalWidth = tileWidth * columns;
        Vector2 currentPosition = new Vector2(startPosition.x - totalWidth / 2 + tileWidth / 2, startPosition.y);

        // First row: Place the intro prefab in the center and random level prefabs on the sides  
        int introColumn = columns / 2; // choose center column index for the intro
        for (int col = 0; col < columns; col++)
        {
            GameObject tile;
            if (col == introColumn && introPrefab != null)
            {
                // Spawn intro prefab in the center
                tile = Instantiate(introPrefab, currentPosition, Quaternion.identity);
            }
            else
            {
                // Spawn a random level prefab
                GameObject selectedPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Length)];
                tile = Instantiate(selectedPrefab, currentPosition, Quaternion.identity);
            }
            tile.transform.parent = transform;

            // Move to the next column position
            currentPosition.x += tileWidth;
        }

        // Additional rows: Spawn random level prefabs 
        // Reset x-position to the start of the row and move one row down
        currentPosition.x = startPosition.x - totalWidth / 2 + tileWidth / 2;
        currentPosition.y -= tileHeight;

        for (int row = 1; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject selectedPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Length)];
                GameObject tile = Instantiate(selectedPrefab, currentPosition, Quaternion.identity);
                tile.transform.parent = transform;

                currentPosition.x += tileWidth;
            }
            // Reset x to the start and move down one row
            currentPosition.x = startPosition.x - totalWidth / 2 + tileWidth / 2;
            currentPosition.y -= tileHeight;
        }

        // Spawn a death trigger below the level
        SpawnDeathTrigger(totalWidth, currentPosition.y, startPosition.x - totalWidth / 2);
    }

    private void SpawnDeathTrigger(float width, float yPos, float levelStartX)
    {
        if (deathTriggerPrefab != null)
        {
            Vector2 spawnPosition = new Vector2(levelStartX + width / 2, yPos - 1f);
            GameObject deathTrigger = Instantiate(deathTriggerPrefab, spawnPosition, Quaternion.identity);
            deathTrigger.transform.localScale = new Vector3(width * 4, 1f, 1f); // The DeathTrigger is 4x times the size of the level
        }
    }

    private Bounds GetPrefabBounds(GameObject prefab)
    {
        // Instantiate a temporary object to measure its bounds
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
