using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraMoveFunction : MonoBehaviour
{
    CinemachineVirtualCamera _vcam;

    private void Awake()
    {
        _vcam = this.transform.GetComponent<CinemachineVirtualCamera>();
    }

    public void DecreaseVibration(float time)
    {
        StartCoroutine(DecreaseVibrationCoroutine(time));
        GameManager.Instance.hapticManager.RumbleGamePad(0.2f, time, false);
    }

    public IEnumerator DecreaseVibrationCoroutine(float time)
    {
        float originTime = time;
        CinemachineImpulseListener listener = _vcam.gameObject.GetComponent<CinemachineImpulseListener>();
        while (time > 0f)
        {
            time -= Time.deltaTime;
            listener.m_Gain = Mathf.Lerp(0f, 1f, time / originTime);
            yield return null;
        }
        listener.m_Gain = 0f;
    }

    public void IncreaseVibration(float time)
    {
        StartCoroutine(IncreaseVibrationCoroutine(time));
        GameManager.Instance.hapticManager.RumbleGamePad(0.2f, time, true);
    }

    public IEnumerator IncreaseVibrationCoroutine(float time)
    {
        float originTime = time;
        CinemachineImpulseListener listener = _vcam.gameObject.GetComponent<CinemachineImpulseListener>();
        while (time > 0f)
        {
            time -= Time.deltaTime;
            listener.m_Gain = Mathf.Lerp(0.5f, 0f, time / originTime);
            yield return null;
        }
        listener.m_Gain = 0.5f;
    }
}
