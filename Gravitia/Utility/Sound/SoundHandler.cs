using System.Collections;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    // �ִϸ��̼� �̺�Ʈ��
    public void PlayEvent(string _eventName)
    {
        AkSoundEngine.PostEvent(_eventName, this.gameObject);
    }

}
