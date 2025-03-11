using UnityEngine;

public class TownEntryChecker : MonoBehaviour
{
    [SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] private bool isInTown = true;
    [SerializeField] private GameObject postProcessingTown;
    [SerializeField] private GameObject postProcessingMine;

    private void Start()
    {
        UpdateMusic();
    }

    public void TogglePostProcessingSwitch()
    {
        isInTown = !isInTown;
        postProcessingTown.SetActive(isInTown);
        postProcessingMine.SetActive(!isInTown);

        UpdateMusic();
    }

    private void UpdateMusic()
    {
        if (musicPlayer != null)
        {
            musicPlayer.SetTownStatus(isInTown);
        }
    }
}
