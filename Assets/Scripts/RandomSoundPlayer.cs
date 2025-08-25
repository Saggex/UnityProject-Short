using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [Header("Sound Clips")]
    [Tooltip("Liste an Sounds, aus denen zufällig ausgewählt wird.")]
    public AudioClip[] soundClips;

    [Header("Random Settings")]
    [Range(0f, 1f)] public float minVolume = 0.8f;
    [Range(0f, 1f)] public float maxVolume = 1f;

    [Range(-3f, 3f)] public float minPitch = 0.9f;
    [Range(-3f, 3f)] public float maxPitch = 1.1f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Spielt zufällig einen der angegebenen Sounds mit zufälliger Lautstärke und Pitch ab.
    /// </summary>
    public void PlayRandomSound()
    {
        if (soundClips.Length == 0)
        {
            Debug.LogWarning($"[{nameof(RandomSoundPlayer)}] Keine Sounds zugewiesen!");
            return;
        }

        int index = Random.Range(0, soundClips.Length);
        AudioClip clip = soundClips[index];

        float volume = Random.Range(minVolume, maxVolume);
        float pitch = Random.Range(minPitch, maxPitch);

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
    }
}
