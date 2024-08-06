using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public void MusicTrigger(string musicEvent)
    {
        if(musicEvent != null)
        {
            AkSoundEngine.PostEvent(musicEvent, gameObject);
        }
    }

    public void MusicSwitch(string switchNum)
    {
        if (switchNum != null)
        {
            AkSoundEngine.SetSwitch("MusicVar", switchNum, gameObject);
        }
    }

    public void MusicFadeOut()
    {
        AkSoundEngine.PostEvent("MusicFadeOut", gameObject);
    }

}
