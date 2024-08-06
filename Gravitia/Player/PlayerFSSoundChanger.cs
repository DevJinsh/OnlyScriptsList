using AK.Wwise;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFSSoundChanger : MonoBehaviour
{
    //Tag5 = Sound_Stone
    //Tag6 = Sound_Wood
    //Tag7 = Sound_Metal
    //Tag8 = Sound_Dirt

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Sound_Wood"))
        {
            AkSoundEngine.SetSwitch("CHR_FS", "Wood", gameObject);
        }
        else if (collision.gameObject.CompareTag("Sound_Metal"))
        {
            AkSoundEngine.SetSwitch("CHR_FS", "Metal", gameObject);
        }
        else if (collision.gameObject.CompareTag("Sound_Dirt"))
        {
            AkSoundEngine.SetSwitch("CHR_FS", "Dirt", gameObject);
        }
        else
        {
            AkSoundEngine.SetSwitch("CHR_FS", "Stone", gameObject);
        }
    }

}
