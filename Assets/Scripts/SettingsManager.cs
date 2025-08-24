using UnityEngine;

/// <summary>
/// Stores and applies player configurable settings such as audio volume.
/// Values are persisted via <see cref="PlayerPrefs"/>.
/// </summary>
public class SettingsManager : PersistentSingleton<SettingsManager>
{
    private const string MusicVolKey = "MusicVolume";
    private const string SfxVolKey = "SfxVolume";

    public float MusicVolume { get; private set; } = 1f;
    public float SfxVolume { get; private set; } = 1f;

    protected override void Awake()
    {
        base.Awake();
        Load();
        Apply();
    }

    private void Load()
    {
        MusicVolume = PlayerPrefs.GetFloat(MusicVolKey, 1f);
        SfxVolume = PlayerPrefs.GetFloat(SfxVolKey, 1f);
    }

    private void Apply()
    {
        SoundManager.Instance.SetMusicVolume(MusicVolume);
        SoundManager.Instance.SetSFXVolume(SfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        SoundManager.Instance.SetMusicVolume(MusicVolume);
        PlayerPrefs.SetFloat(MusicVolKey, MusicVolume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float value)
    {
        SfxVolume = Mathf.Clamp01(value);
        SoundManager.Instance.SetSFXVolume(SfxVolume);
        PlayerPrefs.SetFloat(SfxVolKey, SfxVolume);
        PlayerPrefs.Save();
    }
}
