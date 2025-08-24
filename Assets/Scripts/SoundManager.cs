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
    [SerializeField, Range(0f,1f)] private float musicVolume = 1f;
    [SerializeField, Range(0f,1f)] private float sfxVolume = 1f;

    private void Start()
    {
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

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

    /// <summary>
    /// Sets the music channel volume (0-1).
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    /// <summary>
    /// Sets the sound effect channel volume (0-1).
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
}
