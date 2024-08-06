using Beautify.Universal;
using Michsky.UI.Heat;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public OptionData optionData;

    public SliderManager masterVolumeSlider;
    public SliderManager musicVolumeSlider;
    public SliderManager sfxVolumeSlider;
    public Dropdown selectResolution;
    public SliderManager brightSlider;
    public Dropdown selectLanguage;
    public SwitchManager useHaptic;

    private OptionData defaultValue = new OptionData(100f, 100f, 100f, 1, 0f, 1, true);

    private void Start()
    {
        DataLoad();
    }

    public void DataLoad()
    {
        Debug.Log("데이터 불러오기");
        DataSync(SaveSystem.LoadOptionData() ?? defaultValue);
    }

    public void DataSave() // 설정창 닫을 때 저장
    {
        Debug.Log("데이터 저장");
        SaveSystem.SaveOptionData(this);
    }

    private void DataSync(OptionData data)
    {
        optionData = data;

        masterVolumeSlider.mainSlider.value = optionData.masterVolume;
        musicVolumeSlider.mainSlider.value = optionData.musicVolume;
        sfxVolumeSlider.mainSlider.value = optionData.sfxVolume;
        selectResolution.selectedItemIndex = optionData.resolution;
        brightSlider.mainSlider.value = optionData.bright;
        selectLanguage.selectedItemIndex = optionData.language;
        useHaptic.isOn = optionData.useHaptic;
        FindAnyObjectByType<HapticManager>().isGamePadRumble = optionData.useHaptic;
        FindAnyObjectByType<GraphicsManager>().SetResolution(optionData.resolution);
    }

    private void Update()
    {
        BeautifySettings.settings.brightness.value = 1 + brightSlider.mainSlider.value * 0.1f;
    }
}
