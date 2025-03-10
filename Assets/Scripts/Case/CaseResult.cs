using UnityEngine;

public class CaseResult : MonoBehaviour
{
    [SerializeField] private Hotbar hotbar;

    private InventoryItem itemWon;
    private RectTransform rectTransform;

    private void Start()
    {
        // Get the RectTransform component of this UI element
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowResult()
    {
        // Check if this UI element overlaps with another UI element
        CaseCell caseCell = GetOverlappingCaseCell();
        if (caseCell != null)
        {
            itemWon = caseCell.itemInThisCell;
        }
        else
        {
            itemWon = null;
        }

        if (itemWon != null)
        {
            hotbar.AddItemToHotbar(itemWon);
            hotbar.EquipItem(itemWon);
        }
    }

    private CaseCell GetOverlappingCaseCell()
    {
        // Get all CaseCell components in the scene
        CaseCell[] caseCells = FindObjectsOfType<CaseCell>();

        // Loop through each CaseCell and check if its RectTransform overlaps with this UI element's RectTransform
        foreach (CaseCell caseCell in caseCells)
        {
            RectTransform caseCellRectTransform = caseCell.transform.parent.GetComponent<RectTransform>();

            // Calculate the intersection of the two Rectangles
            Rect intersection = Rect.zero;
            if (RectanglesIntersect(rectTransform, caseCellRectTransform, out intersection))
            {
                // If the intersection area is greater than a certain threshold, consider it an overlap
                float intersectionArea = intersection.width * intersection.height;
                float rectTransformArea = rectTransform.rect.width * rectTransform.rect.height;
                if (intersectionArea > 0.2f * rectTransformArea) // Adjust the threshold as needed
                {
                    return caseCell;
                }
            }
        }

        return null;
    }

    private bool RectanglesIntersect(RectTransform rectTransform1, RectTransform rectTransform2, out Rect intersection)
    {
        Vector3[] corners1 = new Vector3[4];
        rectTransform1.GetWorldCorners(corners1);

        Vector3[] corners2 = new Vector3[4];
        rectTransform2.GetWorldCorners(corners2);

        Vector3 bottomLeft = Vector3.Max(corners1[0], corners2[0]);
        Vector3 topRight = Vector3.Min(corners1[2], corners2[2]);

        if (bottomLeft.x < topRight.x && bottomLeft.y < topRight.y)
        {
            intersection = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
            return true;
        }
        else
        {
            intersection = new Rect();
            return false;
        }
    }
}
