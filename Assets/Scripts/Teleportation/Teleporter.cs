using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject keyPressPopUp;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject player;
    [SerializeField] private TownEntryChecker townEntryChecker;
    [SerializeField] private UnityEvent generalEvent;
    // TODO: Remove these bools with Unity Event call
    [SerializeField] private bool shouldSwitchPostProcess = false;
    [SerializeField] private bool shouldSwitchMusic = false;
    [SerializeField] private bool shouldCountEnemiesForMusic = false;

    [Header("UI")]
    [SerializeField] private GameObject switchScreen;
    [SerializeField] private string locationName;
    [SerializeField] private TextMeshProUGUI locationNametext;


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
        if (MenuManager.Instance.inMenu) return;


        if (inTeleportRange)
            TeleportableState();
    }

    private void TeleportableState()
    {
        if (Input.GetKeyDown(KeyCode.E) && BasePlayer.Instance.PlayerCanTeleport())
        {
            BasePlayer.Instance.SetCanTeleport(false);
            BasePlayer.Instance.stateMachine.ChangeState(BasePlayer.Instance.idleState);
            generalEvent?.Invoke();
            switchScreen.SetActive(true);
            StartCoroutine(Teleport());
        }
    }


    private IEnumerator Teleport()
    {
        Animator switchAnim = switchScreen.GetComponent<Animator>();
        switchAnim.SetBool("LocationSwap", true);
        locationNametext.text = locationName;

        yield return new WaitForSeconds(1);

        player.transform.position = target.transform.position;

        // Switch Post-Processing effects if needed
        if (shouldSwitchPostProcess)
            townEntryChecker.TogglePostProcessingSwitch();

        // Switch the music to the selected playlist if shouldSwitchMusic is true
        if (shouldSwitchMusic)
            SwitchMusic();

        if (shouldCountEnemiesForMusic)
            musicPlayer.CountEnemies();

        yield return new WaitForSeconds(3);

        switchAnim.SetBool("LocationSwap", false);
        BasePlayer.Instance.SetCanTeleport(true);
        switchScreen.SetActive(false);
    }


    private void SwitchMusic()
    {
        if (musicPlayer != null)
        {
            musicPlayer.isInSpecialArea = true; // Ensure it's in a special area

            switch (selectedPlaylist)
            {
                case PlaylistType.Town:
                    musicPlayer.SetSpecialZoneStatus(true, MusicPlayer.StartingLocation.Town);
                    break;
                case PlaylistType.Casino:
                    musicPlayer.SetSpecialZoneStatus(true, MusicPlayer.StartingLocation.Casino);
                    break;
                case PlaylistType.Shop:
                    musicPlayer.SetSpecialZoneStatus(true, MusicPlayer.StartingLocation.Shop);
                    break;
                default:
                    musicPlayer.isInSpecialArea = false;
                    musicPlayer.SwitchPlaylist(musicPlayer.ambientClips);
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
