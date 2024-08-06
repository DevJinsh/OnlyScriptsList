using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] public Slider MasterSlider;
    [SerializeField] public Slider MusicSlider;
    [SerializeField] public Slider SFXSlider;

    private void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }

        AkSoundEngine.SetState("Place", "OutDoor");
           
    }
    private void Update()
    {
        AkSoundEngine.SetRTPCValue("MasterVolume", MasterSlider.value);
        AkSoundEngine.SetRTPCValue("MusicVolume", MusicSlider.value);
        AkSoundEngine.SetRTPCValue("SFXVolume", SFXSlider.value);
    }

    /// <summary>
    /// 사운드 이벤트 호출 함수.
    /// 
    /// </summary>
    /// <param name="_soundName"></param>
    public void SoundPlay(string _soundName, GameObject emitter)
    {
        AkSoundEngine.PostEvent(_soundName, emitter);
    }

    public void Destruct(string material, GameObject destructObj)
    {
        switch(material)
        {
            case "Metal":
                AkSoundEngine.PostEvent("MetalDestruct", destructObj);
                break;
            case "PlasticBag":
                AkSoundEngine.PostEvent("PlasticBagDestruct", destructObj);
                break;

        }
    }

    public void SoundDeadAfter()
    {
        AkSoundEngine.PostEvent("Dead", gameObject);

    }

    public void TitleSoundSKip()
    {
        AkSoundEngine.PostEvent("TitlleSkip", gameObject);

    }

}
