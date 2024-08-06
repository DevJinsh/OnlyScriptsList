using System;

[Serializable]
public class OptionData
{
    //Sound
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    //Visual
    public int resolution;
    public float bright;
    //Language
    public int language;
    //haptic
    public bool useHaptic;
    public OptionData(float masterVolumeValue, float musicVolumeValue, float sfxVolumeValue, int resolutionValue, float brightValue, int languageValue, bool useHapticValue)
    {
        masterVolume = masterVolumeValue;
        musicVolume = musicVolumeValue;
        sfxVolume = sfxVolumeValue;
        resolution = resolutionValue;
        bright = brightValue;
        language = languageValue;
        useHaptic = useHapticValue;
    }
}
