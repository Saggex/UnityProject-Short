using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Bridges UI controls to the <see cref="SettingsManager"/>.
/// Intended to be placed on an options menu panel.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (musicSlider != null)
            musicSlider.value = SettingsManager.Instance.MusicVolume;
        if (sfxSlider != null)
            sfxSlider.value = SettingsManager.Instance.SfxVolume;
    }

    public void OnMusicVolumeChanged(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
    }

    public void OnSfxVolumeChanged(float value)
    {
        SettingsManager.Instance.SetSfxVolume(value);
    }
}
