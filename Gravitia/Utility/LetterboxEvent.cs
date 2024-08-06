using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterboxEvent : MonoBehaviour
{
    private GameObject globalVolume;
    // Start is called before the first frame update
    void Start()
    {
        globalVolume = GameObject.Find("Global Volume");
    }

    public void StartLerp00()
    {
        globalVolume.GetComponent<LetterboxManager>().StartLerp00();
    }
    public void StartLerp01()
    {
        globalVolume.GetComponent<LetterboxManager>().StartLerp01();
    }public void StartLerp01Intro()
    {
        globalVolume.GetComponent<LetterboxManager>().StartLerp01Intro();
    }
    public void StartLerp05()
    {
        globalVolume.GetComponent<LetterboxManager>().StartLerp05();
    }
    public void StartLerp05Outro()
    {
        globalVolume.GetComponent<LetterboxManager>().StartLerp05Outro();
    }


    public void SetLBoxValue(float value)
    {
        globalVolume.GetComponent<LetterboxManager>().lbox = value;
    }



    public void DistroyThis()
    {
        Destroy(this);
    }
}

