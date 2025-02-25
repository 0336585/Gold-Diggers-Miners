using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject lastOutlineObject = null; // Tracks the last outline object

    private bool miningEnabled = true;
    private bool armIsFlipped = false;

    [Header("References")]
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject outlinePrefab; // Prefab for the outline effect

    [Header("Mining Info")]
    [SerializeField] private float raycastDistance = 2f; // Distance of the raycast
    [SerializeField] private LayerMask ignoredLayer;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        ArmRotation();
        if (miningEnabled)
        {
            PerformRaycast(); // Always check for outlining
            CheckMining();    // Checks for mining when clicking
        }
    }

    private void ArmRotation()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - arm.transform.parent.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        // Check if the parent is flipped (rotated 180 degrees on Y-axis)
        if (arm.transform.parent.localRotation.y < 0)
        {
            if (!armIsFlipped)
            {
                armIsFlipped = true;
                arm.transform.localScale = new Vector3(arm.transform.localScale.x * -1, arm.transform.localScale.y, arm.transform.localScale.z);
            }

            angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg; ;
        }
        else
        {
            if (armIsFlipped)
            {
                armIsFlipped = false;
                arm.transform.localScale = new Vector3(arm.transform.localScale.x * -1, arm.transform.localScale.y, arm.transform.localScale.z);
            }
        }

        angle = Mathf.Clamp(angle, -90f, 90f);


        arm.transform.rotation = Quaternion.Euler(0, 0, angle);
    }



    private void PerformRaycast()
    {
        Vector2 origin = transform.position;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance, ~ignoredLayer);

        if (hit.collider != null)
        {
            NoteHealth noteHealth = hit.collider.GetComponent<NoteHealth>();

            if (noteHealth != null) // Only outline tiles with NoteHealth
            {
                if (lastOutlineObject == null || lastOutlineObject.transform.position != hit.collider.transform.position)
                {
                    RemoveOutline(); // Remove old outline before adding a new one
                    CreateOutline(hit.collider.gameObject);
                }
            }
        }
        else
        {
            RemoveOutline(); // Remove outline if nothing is hit
        }

        Debug.DrawRay(origin, direction * raycastDistance, Color.red);
    }

    private void CheckMining()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Vector2 origin = transform.position;
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 direction = (mousePosition - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance, ~ignoredLayer);

            if (hit.collider != null)
            {
                Debug.Log("Raycast hit something");

                NoteHealth noteHealth = hit.collider.GetComponent<NoteHealth>();
                if (noteHealth != null)
                {
                    Debug.Log("Hit an object with NoteHealth: " + hit.collider.gameObject.name);
                    noteHealth.DamageNote(1);
                }
            }

            Debug.DrawRay(origin, direction * raycastDistance, Color.red, 0.2f);
        }
    }

    private void CreateOutline(GameObject target)
    {
        if (outlinePrefab == null) return;

        lastOutlineObject = Instantiate(outlinePrefab, target.transform.position, Quaternion.identity);
        //lastOutlineObject.transform.localScale = target.transform.localScale * 1.1f; // Slightly bigger for the outline effect
        lastOutlineObject.transform.SetParent(target.transform); // Attach it to the target
    }

    private void RemoveOutline()
    {
        if (lastOutlineObject != null)
        {
            Destroy(lastOutlineObject);
            lastOutlineObject = null;
        }
    }

    public void EnableMining(bool _canMine)
    {
        miningEnabled = _canMine;
    }
}
