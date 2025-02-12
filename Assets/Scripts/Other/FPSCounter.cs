using System.Collections;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    // Taken from here and modified: https://gist.github.com/mstevenson/5103365

    [SerializeField] private bool showFPS = false;
    [SerializeField] private int counterSize = 16;
    [SerializeField] private Color counterColor = Color.magenta;

    private float count;
    private GUIStyle style = new GUIStyle();

    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            // Invert the boolean value
            showFPS = !showFPS;
        }
    }

    private void OnGUI()
    {
        if (showFPS)
        {
            // Position the label at the top-center of the screen
            float xPosition = (Screen.width - 100) / 2; // Center horizontally
            float yPosition = 10; // High up on the screen

            style.normal.textColor = counterColor;
            style.fontSize = counterSize;

            // Create the label with updated position
            GUI.Label(new Rect(xPosition, yPosition, 100, 25), "FPS: " + Mathf.Round(count), style);
        }
    }
}

