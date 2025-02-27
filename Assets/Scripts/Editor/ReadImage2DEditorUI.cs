using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReadImage2D))]
public class ReadImage2DEditorUI : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get a reference to the ReadImage2D component
        ReadImage2D readImage2D = (ReadImage2D)target;

        // Add a button to the Inspector
        if (GUILayout.Button("Scan Image and Generate Level"))
        {
            // Call the ScanImage method
            readImage2D.ScanImage();
        }
    }
}
