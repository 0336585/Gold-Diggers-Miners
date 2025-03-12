using TMPro;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject keyPressPopUp;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject player;
    [SerializeField] private TownEntryChecker townEntryChecker;
    [SerializeField] private bool shouldSwitchPostProcess = false;
    [SerializeField] private bool shouldSwitchMusic = false;
    [SerializeField] private bool shouldCountEnemiesForMusic = false;

    // New Field for MusicPlayer reference
    [SerializeField] private MusicPlayer musicPlayer;

    // Enum for selecting which playlist to switch to
    public enum PlaylistType
    {
        None,
        Ambient,
        Action,
        Town,
        Casino,
        Shop
    }

    [SerializeField] private PlaylistType selectedPlaylist = PlaylistType.None;
    private bool inTeleportRange;

    private void Update()
    {
        if (inTeleportRange)
            TeleportableState();
    }

    private void TeleportableState()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        player.transform.position = target.transform.position;

        // Switch Post-Processing effects if needed
        if (shouldSwitchPostProcess)
            townEntryChecker.TogglePostProcessingSwitch();

        // Switch the music to the selected playlist if shouldSwitchMusic is true
        if (shouldSwitchMusic)
            SwitchMusic();

        if (shouldCountEnemiesForMusic)
            musicPlayer.CountEnemies();
    }

    private void SwitchMusic()
    {
        if (musicPlayer != null)
        {
            // Select the playlist based on the selected enum value
            switch (selectedPlaylist)
            {
                case PlaylistType.Ambient:
                    musicPlayer.SwitchPlaylist(musicPlayer.ambientClips);
                    musicPlayer.isInSpecialArea = false;
                    break;
                case PlaylistType.Action:
                    musicPlayer.SwitchPlaylist(musicPlayer.actionClips);
                    musicPlayer.isInSpecialArea = false;
                    break;
                case PlaylistType.Town:
                    musicPlayer.SwitchPlaylist(musicPlayer.townClips);
                    musicPlayer.isInSpecialArea = true;
                    break;
                case PlaylistType.Casino:
                    musicPlayer.SwitchPlaylist(musicPlayer.casinoClips);
                    musicPlayer.isInSpecialArea = true;
                    break;
                case PlaylistType.Shop:
                    musicPlayer.SwitchPlaylist(musicPlayer.shopClips);
                    musicPlayer.isInSpecialArea = true;
                    break;
                default:
                    // If None is selected, do nothing or switch to a default
                    musicPlayer.SwitchPlaylist(musicPlayer.ambientClips);
                    musicPlayer.isInSpecialArea = false;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inTeleportRange = true;
            keyPressPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BasePlayer>())
        {
            inTeleportRange = false;
            keyPressPopUp.SetActive(false);
        }
    }
}
