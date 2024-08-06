using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GamepadExtension
{
	public static void SetMotorSpeeds(this Gamepad gamepad, float lowFrequency, float highFrequency, float seconds)
	{
		gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
		GameManager.Instance.StartCoroutine(WaitActionForSeconds(seconds, () => gamepad.SetMotorSpeeds(0, 0)));
	}

	private static IEnumerator WaitActionForSeconds(float seconds, System.Action action)
	{
		yield return new WaitForSeconds(seconds);
		action();
	}
}
