using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticManager : MonoBehaviour
{

    public bool isGamePadRumble = true;

    private Coroutine rumbleGamePad;
    private float currentRumblingPower = 0;
    public void UseHaptic(bool e)
    {
        isGamePadRumble = e;
    }
    public void RumbleGamePad(float power, float during, bool isIncrease = false)
    {
        if (Gamepad.current == null)
            return;
        if (GameManager.Instance.CurrentDevice == GameManager.Device.GamePad && isGamePadRumble)
        {
            if (currentRumblingPower > power)
                return;
            if (rumbleGamePad != null)
                StopCoroutine(rumbleGamePad);
            if (during == Mathf.Infinity)
                Gamepad.current?.SetMotorSpeeds(power, power);
            else
                rumbleGamePad = StartCoroutine(Rumbling(power, during, isIncrease));
        }
    }

    IEnumerator Rumbling(float power, float during, bool isIncrease)
    {
        float currentTime = 0f;
        float adjustPower;
        if (isIncrease)
            currentRumblingPower = power;
        else
            currentRumblingPower = 0f;
        while (currentTime <= during)
        {
            yield return null;
            if (isIncrease)
                adjustPower = Mathf.Lerp(0f, power, currentTime / during);
            else
                adjustPower = Mathf.Lerp(power, 0f, currentTime / during);
            Gamepad.current?.SetMotorSpeeds(adjustPower, adjustPower);
            currentTime += Time.deltaTime;
        }
        if (isIncrease)
            adjustPower = power;
        else
            adjustPower = 0f;
        Gamepad.current?.SetMotorSpeeds(adjustPower, adjustPower);
        rumbleGamePad = null;
    }

    public void StopRumbling()
    {
        Gamepad.current?.SetMotorSpeeds(0f, 0f);
    }

    private void OnDisable()
    {
        Gamepad.current?.SetMotorSpeeds(0f,0f);
    }
}
