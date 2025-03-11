using UnityEngine;

public class TownEntryChecker : MonoBehaviour
{
    [SerializeField] private bool isInTown = true;
    [SerializeField] private GameObject postProcessingTown;
    [SerializeField] private GameObject postProcessingMine;

    public void TogglePostProcessingSwitch()
    {
        isInTown = !isInTown;

        // Enable the correct post-processing object and disable the other
        postProcessingTown.SetActive(isInTown);
        postProcessingMine.SetActive(!isInTown);
    }
}
