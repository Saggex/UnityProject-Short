using UnityEngine;

/// <summary>
/// Plays background music, ambient sounds, and interaction cues.
/// </summary>
public class SoundManager : PersistentSingleton<SoundManager>
{
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            return;
        }
    }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    /// <summary>
    /// Plays a looping background track.
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// Stops the current music track.
    /// </summary>
    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }

    /// <summary>
    /// Plays a one-shot sound effect.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
