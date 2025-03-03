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
    private BasePlayer player;
    private CustomCursor customCursor;

    [Header("Mining Info")]
    [SerializeField] private float raycastDistance = 2f; // Distance of the raycast
    [SerializeField] private LayerMask ignoredLayer;

    private void Awake()
    {
        customCursor = GetComponent<CustomCursor>();
    }

    void Start()
    {
        mainCamera = Camera.main;

        player = GetComponent<BasePlayer>();
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

        // Flip player if the mouse is behind them
        bool shouldFlip = mousePosition.x < arm.transform.parent.position.x;
        if (shouldFlip != armIsFlipped)
        {
            armIsFlipped = shouldFlip;
            player.Flip();
            arm.transform.localScale = new Vector3(-arm.transform.localScale.x, arm.transform.localScale.y, arm.transform.localScale.z);
        }

        // Adjust angle when flipped
        if (armIsFlipped)
        {
            angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        }

        // Clamp angle to prevent unnatural rotations
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
            NodeHealth noteHealth = hit.collider.GetComponent<NodeHealth>();

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

                NodeHealth noteHealth = hit.collider.GetComponent<NodeHealth>();
                if (noteHealth != null)
                {
                    Debug.Log("Hit an object with NoteHealth: " + hit.collider.gameObject.name);
                    noteHealth.DamageNode(1);
                }
            }

            Debug.DrawRay(origin, direction * raycastDistance, Color.red, 0.2f);
        }
    }

    private void CreateOutline(GameObject target)
    {
        if (outlinePrefab == null) return;

        customCursor.SelectMiningCursor();

        lastOutlineObject = Instantiate(outlinePrefab, target.transform.position, Quaternion.identity);
        //lastOutlineObject.transform.localScale = target.transform.localScale * 1.1f; // Slightly bigger for the outline effect
        lastOutlineObject.transform.SetParent(target.transform); // Attach it to the target
    }

    public void RemoveOutline()
    {
        if (lastOutlineObject != null)
        {
            Destroy(lastOutlineObject);
            lastOutlineObject = null;
        }

        customCursor.SelectStandardCursor();
    }

    public void EnableMining(bool _canMine)
    {
        miningEnabled = _canMine;
    }
}
