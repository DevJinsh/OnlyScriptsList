using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beautify.Universal;

public class LetterboxManager : MonoBehaviour
{
    public float lbox;
    public bool isLerping;

    private float targetValue;
    private float durationl;
    public void LetterOn()
    {
        lbox = BeautifySettings.settings.frameBandVerticalSize.value;
    }

    public void FixedUpdate()
    {
        BeautifySettings.settings.frameBandVerticalSize.value = lbox;
    }
    public void StartLerp01Intro()
    {
        if (!isLerping)
        {
            StartCoroutine(LerpValue(.1f, 6));
        }
    }public void StartLerp01()
    {
        if (!isLerping)
        {
            StartCoroutine(LerpValue(.1f, 1.5f));
        }
    }
    public void StartLerp00()
    {
        if (!isLerping)
        {
            StartCoroutine(LerpValue(0, 1.5f));
        }
    }
    public void StartLerp05()
    {
        if (!isLerping)
        {
            StartCoroutine(LerpValue(.5f, 2));
        }
    }
    public void StartLerp05Outro()
    {
        if (!isLerping)
        {
            StartCoroutine(LerpValue(.5f, 4));
        }
    }


    IEnumerator LerpValue(float targetValue, float duration)
    {
        isLerping = true;
        float startValue = lbox;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            lbox = Mathf.Lerp(startValue, targetValue, (Time.time - startTime) / duration);
            yield return null;
        }

        lbox = targetValue;
        isLerping = false;
    }
}
