using UnityEngine;

public class ReadImage2D : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private Texture2D image;

    [Header("Game Objects")]
    [SerializeField] private MapPixel[] gameobjects;
    [SerializeField] private MapPixel exit;

    [Header("Offset")]
    [SerializeField] private float gridSize = 1.5f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;

    private Color[] pix;

    // Scan to prevent bad maps from being instantiated
    public void ScanImage()
    {
        pix = image.GetPixels();

        if (GetColorCount(pix, exit.GetPixel) > 1)
        {
            Debug.Assert(GetColorCount(pix, exit.GetPixel) > 1, "There are multiple exit pixels >:(");
        }
        else
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        int worldX = image.width;
        int worldY = image.height;

        // Create a parent object to store all the created prefabs in
        GameObject level = new GameObject("Level");

        // Calculate the offset to center the level
        float offsetX = (worldX * gridSize) / 2 - xOffset;
        float offsetY = (worldY * gridSize) / 2 - yOffset;

        // Adjust the starting position to be centered based on the image width and height
        Vector2 startingSpawnPosition = new Vector2(-offsetX, -offsetY);

        Vector2 currentSpawnPos = startingSpawnPosition;

        // Track occupied positions
        bool[,] occupiedPositions = new bool[worldX, worldY];

        // Look through the grid from left to right, bottom to top
        for (int y = 0; y < worldY; y++)
        {
            for (int x = 0; x < worldX; x++)
            {
                if (occupiedPositions[x, y])
                {
                    currentSpawnPos.x += gridSize;
                    continue; // Skip if the position is already occupied by a larger object
                }

                Color c = pix[y * worldX + x];

                // Look through all the individual pixels, and instantiate when necessary and then spawn objects
                foreach (MapPixel mapPixel in gameobjects)
                {
                    if (c == mapPixel.GetPixel)
                    {
                        Instantiate(mapPixel.GetGameObject, new Vector3(currentSpawnPos.x, currentSpawnPos.y, 0), Quaternion.identity, level.transform);
                    }
                }

                currentSpawnPos.x += gridSize;
            }

            currentSpawnPos.x = startingSpawnPosition.x;
            currentSpawnPos.y += gridSize;
        }
    }

    private int GetColorCount(Color[] image, Color color)
    {
        int count = 0;
        for (int i = 0; i < image.Length; i++)
        {
            if (image[i] == color)
            {
                count++;
            }
        }
        return count;
    }
}