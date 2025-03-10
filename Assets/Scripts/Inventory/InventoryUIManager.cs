using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [SerializeField] private Material uncommonMat;
    [SerializeField] private Material rareMat;
    [SerializeField] private Material epicMat;
    [SerializeField] private Material legendaryMat;
    [SerializeField] private Material mythicMat;
    [SerializeField] private Material secretMat;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate inventory instances
            return;
        }

        Instance = this;
    }

    public Material UncommonMat
    {
        get { return uncommonMat; }
        private set { uncommonMat = value; }
    }

    public Material RareMat
    {
        get { return rareMat; }
        private set { rareMat = value; }
    }

    public Material EpicMat
    {
        get { return epicMat; }
        private set { epicMat = value; }
    }

    public Material LegendaryMat
    {
        get { return legendaryMat; }
        private set { legendaryMat = value; }
    }

    public Material MythicMat
    {
        get { return mythicMat; }
        private set { mythicMat = value; }
    }

    public Material SecretMat
    {
        get { return secretMat; }
        private set { secretMat = value; }
    }
}
