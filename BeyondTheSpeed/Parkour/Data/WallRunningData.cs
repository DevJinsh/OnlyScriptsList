using UnityEngine;

[CreateAssetMenu(fileName = "WallRunningData", menuName = "ScriptableObjects/WallRunningData")]
public class WallRunningData : ScriptableObject
{
	[Header("Wall Running")]
	public float minSpeed = 10f;
	public float wallRunSpeedMultiplier = 1f;
	public float upwardDelta = 25f;
	public float downwardDelta = 1f;
	public float minYVelocity = -5f;
	public float minRuntime = 0.3f;

	public float jumpForceMultiplier = 1.5f;

	[Header("Detection")]
	public float minDotValue;
	public float maxDotValue;

	[Header("CameraMove")]
	public float cameraStartTime;
	public float cameraEndTime;
	public float cameraAngle;
	public float turnDuration;
}