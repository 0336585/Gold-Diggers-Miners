using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D standardCursorTexture;
    public Texture2D miningCursorTexture;
    public Vector2 hotSpot = Vector2.zero; // Adjust this to change the cursor's click position

    void Start()
    {
        SelectStandardCursor();
    }

    public void SelectStandardCursor()
    {
        Cursor.SetCursor(standardCursorTexture, hotSpot, CursorMode.Auto);

    }

    public void SelectMiningCursor()
    {
        Cursor.SetCursor(miningCursorTexture, hotSpot, CursorMode.Auto);
    }
}
