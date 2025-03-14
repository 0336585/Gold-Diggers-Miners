using UnityEngine;

public class TownEntryChecker : MonoBehaviour
{
    [SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] private bool isInTown = true;
    [SerializeField] private GameObject postProcessingTown;
    [SerializeField] private GameObject postProcessingMine;

    public void TogglePostProcessingSwitch()
    {
        isInTown = !isInTown;
        postProcessingTown.SetActive(isInTown);
        postProcessingMine.SetActive(!isInTown);
    }
}
