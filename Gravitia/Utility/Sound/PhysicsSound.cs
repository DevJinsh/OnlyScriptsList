using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSound : MonoBehaviour
{
    public MaterialSound materialSound;
    bool once = false;
    bool isBall = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Projectile"))
        {
            switch (materialSound)
            {
                case MaterialSound.Metal:
                    AkSoundEngine.PostEvent("Metal", gameObject);
                    break;
                case MaterialSound.LightMetal:
                    AkSoundEngine.PostEvent("LightMetal", gameObject);
                    break;
                case MaterialSound.Wood:
                    AkSoundEngine.PostEvent("Wood", gameObject);
                    break;
                case MaterialSound.PaperBox:
                    AkSoundEngine.PostEvent("PaperBox", gameObject);
                    break;
                case MaterialSound.PlasticBag:
                    AkSoundEngine.PostEvent("PlasticBag", gameObject);
                    break;
                case MaterialSound.BigStone:
                    BigStoneSound();
                    break;
                case MaterialSound.Plastic:
                    AkSoundEngine.PostEvent("Plastic", gameObject);
                    break;
                case MaterialSound.MetalBox:
                    AkSoundEngine.PostEvent("MetalBox", gameObject);
                    break;
                default:
                    Debug.Log("Collision Sound is not found!");
                    break;
            }        

        }
    }

    public void BigStoneSound()
    {
        isBall = true;
        if (!once)
        {
            AkSoundEngine.PostEvent("BigStone", gameObject);
            once = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isBall && other.gameObject.CompareTag("Sound_BigStone"))
        {
            AkSoundEngine.PostEvent("Slam", gameObject);
        }
    }

}

public enum MaterialSound
{
    Metal,
    LightMetal,
    Wood,
    PaperBox,
    PlasticBag,
    BigStone,
    Plastic,
    MetalBox
}