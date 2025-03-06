using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] ambientClips;
    [SerializeField] private AudioClip[] actionClips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip defaultClip;
    [SerializeField] private float distanceThreshold = 25f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float combatMusicMinDuration = 3f;
    [SerializeField] private float volumeRecoveryThreshold = 0.4f;
    [SerializeField] private float volumeRecoveryDelay = 4f;
    [SerializeField] private float volumeRecoverySpeed = 0.3f;
    [SerializeField] private int enemyAmountForSwitch = 3;

    private AudioClip[] currentPlaylist;
    private Coroutine fadeCoroutine;
    private Coroutine switchDelayCoroutine;
    private Coroutine volumeRecoveryCoroutine;
    private Transform player;
    private EnemyTargeting[] enemies;

    private int nearbyEnemiesCount = 0;
    private bool canSwitchToAmbient = true;
    private float maxVolume; 

    private void Start()
    {
        enemies = FindObjectsOfType<EnemyTargeting>();
        player = gameObject.transform;
        currentPlaylist = ambientClips;
        maxVolume = audioSource.volume;  
        PlayRandomClip();
        StartCoroutine(MonitorAudioSource());
    }

    private void Update()
    {
        CountNearbyEnemies();

        if (nearbyEnemiesCount > enemyAmountForSwitch && currentPlaylist != actionClips)
        {
            SwitchPlaylist(actionClips);
        }
        else if (nearbyEnemiesCount <= enemyAmountForSwitch && currentPlaylist != ambientClips && canSwitchToAmbient)
        {
            SwitchPlaylist(ambientClips);
        }
    }

    private void CountNearbyEnemies()
    {
        nearbyEnemiesCount = 0;
        foreach (EnemyTargeting enemy in enemies)
        {
            if (enemy != null) // Check if the enemy is still active
            {
                float distanceToEnemy = Vector3.Distance(player.position, enemy.transform.position);
                if (distanceToEnemy <= distanceThreshold)
                {
                    nearbyEnemiesCount++;
                }
            }
        }
    }

    private void SwitchPlaylist(AudioClip[] newPlaylist)
    {
        if (newPlaylist == ambientClips && !canSwitchToAmbient)
        {
            return; // Prevent switching to ambient music if cooldown is active
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // Stop the previous fade if it's still running
        }

        currentPlaylist = newPlaylist;

        if (newPlaylist == actionClips && switchDelayCoroutine != null)
        {
            StopCoroutine(switchDelayCoroutine); // Reset ambient switch cooldown if combat music starts
        }

        fadeCoroutine = StartCoroutine(FadeOutAndSwitch());
    }

    private IEnumerator FadeOutAndSwitch()
    {
        float startVolume = audioSource.volume;

        // Fade out the current audio
        for (float time = 0; time < fadeDuration; time += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, time / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();

        // Switch to a random new clip
        PlayRandomClip();

        // Fade in the new audio
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = startVolume;

        // Trigger a cooldown if switching back to ambient music
        if (currentPlaylist == ambientClips)
        {
            switchDelayCoroutine = StartCoroutine(CombatMusicCooldown());
        }

        // Stop the volume recovery coroutine if it's running
        if (volumeRecoveryCoroutine != null)
        {
            StopCoroutine(volumeRecoveryCoroutine);
            volumeRecoveryCoroutine = null;
        }
    }

    private IEnumerator CombatMusicCooldown()
    {
        canSwitchToAmbient = false;
        yield return new WaitForSeconds(combatMusicMinDuration); // Ensure combat music plays for at least this duration
        canSwitchToAmbient = true;
    }

    private void PlayRandomClip()
    {
        if (currentPlaylist.Length == 0)
        {
            Debug.LogWarning("No music clips in playlist. Playing default clip.");
            audioSource.clip = defaultClip;
        }
        else
        {
            audioSource.clip = currentPlaylist[Random.Range(0, currentPlaylist.Length)];
        }

        audioSource.Play();
    }

    // Monitor the audio source to ensure continuous playback and handle volume recovery
    private IEnumerator MonitorAudioSource()
    {
        float lowVolumeTime = 0f;

        while (true)
        {
            if (!audioSource.isPlaying)
            {
                Debug.LogWarning("AudioSource stopped unexpectedly. Playing default clip.");
                PlayRandomClip();
            }

            // Check for low volume and increment the timer
            if (audioSource.volume < volumeRecoveryThreshold)
            {
                lowVolumeTime += Time.deltaTime;

                if (lowVolumeTime >= volumeRecoveryDelay)
                {
                    if (volumeRecoveryCoroutine == null)
                    {
                        volumeRecoveryCoroutine = StartCoroutine(GraduallyRecoverVolume());
                    }
                }
            }
            else
            {
                lowVolumeTime = 0f; // Reset the timer if volume is above the threshold
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator GraduallyRecoverVolume()
    {
        while (audioSource.volume < maxVolume)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, maxVolume, volumeRecoverySpeed * Time.deltaTime);
            yield return null;
        }

        volumeRecoveryCoroutine = null; // Mark coroutine as finished
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceThreshold);
    }
}
