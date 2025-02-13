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

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Vector2 currentPosition = startPosition;

        // Spawn the intro prefab to the left of the first tile
        if (introPrefab != null)
        {
            GameObject introTile = Instantiate(introPrefab, currentPosition, Quaternion.identity);
            introTile.transform.parent = this.transform;

            // Get intro prefab size and adjust start position
            Bounds introBounds = GetPrefabBounds(introTile);
            currentPosition.x += introBounds.size.x; // Shift to the right
        }

        for (int row = 0; row < rows; row++)
        {
            Vector2 rowStartPosition = currentPosition;

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
                currentPosition.x += tileBounds.size.x;
            }

            // Reset X position to row start and move downward by row height
            currentPosition = rowStartPosition;
            currentPosition.y -= GetPrefabBounds(levelPrefabs[0]).size.y; // Assuming consistent row height
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
