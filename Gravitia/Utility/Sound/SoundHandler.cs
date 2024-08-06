using System.Collections;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    // 애니메이션 이벤트용
    public void PlayEvent(string _eventName)
    {
        AkSoundEngine.PostEvent(_eventName, this.gameObject);
    }

}
