using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RowPrefabs
{
    public GameObject[] prefabs;
}

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 5; // Y-Axis (vertical)
    [SerializeField] private int columns = 5; // X-Axis (horizontal)
    [SerializeField] private Vector2 startPosition = Vector2.zero; // Center position of the grid

    [Header("Prefabs")]
    [SerializeField] private List<RowPrefabs> rowPrefabs; // List of prefabs for each row
    [SerializeField] private GameObject introPrefab;
    [SerializeField] private GameObject deathTriggerPrefab;
    [SerializeField] private GameObject invisibleWallPrefab;

    private GameObject levelParent; // Parent object for all level elements

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Delete old level if it exists
        if (levelParent != null)
        {
            Destroy(levelParent);
        }

        // Create a new parent object for the level
        levelParent = new GameObject("LevelParent");

        // Determine a reference tile size (using the first level prefab as reference)
        float tileWidth = 1f;
        float tileHeight = 1f;
        if (rowPrefabs.Count > 0 && rowPrefabs[0].prefabs.Length > 0)
        {
            tileWidth = GetPrefabBounds(rowPrefabs[0].prefabs[0]).size.x;
            tileHeight = GetPrefabBounds(rowPrefabs[0].prefabs[0]).size.y;
        }

        // Calculate total row width and adjust starting x so that the row is centered
        float totalWidth = tileWidth * columns;
        float totalHeight = tileHeight * rows;
        Vector2 currentPosition = new Vector2(startPosition.x - totalWidth / 2 + tileWidth / 2, startPosition.y);

        // First row: Place the intro prefab in the center and random level prefabs on the sides  
        int introColumn = columns / 2; // choose center column index for the intro
        for (int col = 0; col < columns; col++)
        {
            GameObject tile;
            if (col == introColumn && introPrefab != null)
            {
                tile = Instantiate(introPrefab, currentPosition, Quaternion.identity, levelParent.transform);
            }
            else
            {
                GameObject selectedPrefab = rowPrefabs[0].prefabs[Random.Range(0, rowPrefabs[0].prefabs.Length)];
                tile = Instantiate(selectedPrefab, currentPosition, Quaternion.identity, levelParent.transform);
            }
            currentPosition.x += tileWidth;
        }

        // Additional rows: Spawn random level prefabs 
        currentPosition.x = startPosition.x - totalWidth / 2 + tileWidth / 2;
        currentPosition.y -= tileHeight;

        for (int row = 1; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject selectedPrefab = rowPrefabs[row % rowPrefabs.Count].prefabs[Random.Range(0, rowPrefabs[row % rowPrefabs.Count].prefabs.Length)];
                GameObject tile = Instantiate(selectedPrefab, currentPosition, Quaternion.identity, levelParent.transform);
                currentPosition.x += tileWidth;
            }
            currentPosition.x = startPosition.x - totalWidth / 2 + tileWidth / 2;
            currentPosition.y -= tileHeight;
        }

        // Spawn boundary walls
        InvisibleWallSpawn(totalWidth, totalHeight, startPosition.x, startPosition.y - totalHeight / 2);

        // Spawn a death trigger below the level
        SpawnDeathTrigger(totalWidth, currentPosition.y, startPosition.x - totalWidth / 2);
    }

    private void InvisibleWallSpawn(float width, float height, float centerX, float centerY)
    {
        // Left wall
        InstantiateWall(new Vector2(centerX - width / 2 - 0.5f, centerY), new Vector3(1f, height * 1.5f, 1f));
        // Right wall
        InstantiateWall(new Vector2(centerX + width / 2 + 0.5f, centerY), new Vector3(1f, height * 1.5f, 1f));
        // Top wall 
        InstantiateWall(new Vector2(centerX, centerY + height / 1.7f), new Vector3(width * 1.5f, 1f, 1f));
        // Bottom wall 
        InstantiateWall(new Vector2(centerX, centerY - height / 2.5f), new Vector3(width * 1.5f, 1f, 1f));
    }

    private void InstantiateWall(Vector2 position, Vector3 scale)
    {
        GameObject wall = Instantiate(invisibleWallPrefab, position, Quaternion.identity, levelParent.transform);
        wall.transform.localScale = scale;
    }

    private void SpawnDeathTrigger(float width, float yPos, float levelStartX)
    {
        if (deathTriggerPrefab != null)
        {
            Vector2 spawnPosition = new Vector2(levelStartX + width / 2, yPos - 1f);
            GameObject deathTrigger = Instantiate(deathTriggerPrefab, spawnPosition, Quaternion.identity, levelParent.transform);
            deathTrigger.transform.localScale = new Vector3(width * 4, 1f, 1f); // The DeathTrigger is 4x times the size of the level
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

    public void DeleteLevel()
    {
        if (levelParent != null)
        {
            Destroy(levelParent);
        }
    }

    public void RegenerateLevel()
    {
        DeleteLevel();
        GenerateLevel();
    }
}