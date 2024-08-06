using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementData")]
public class MovementData : ScriptableObject
{
    [Header("Look Around")]
    public float gamePadSensitivity;
    public float mouseSensitivity;
	[Header("Forward Movement")]
    public float firstSpeedThreshold;
    public float secondSpeedThreshold;
    public float thirdSpeedThreshold;
	[Header("Othersie Movement")]
	public float firstSpeedThreshold2;
	public float secondSpeedThreshold2;
	public float thirdSpeedThreshold2;
	[Header("Acceleration")]
    public float firstAccelerationTime;
    public float secondAccelerationTime;
    public float thirdAccelerationTime;
    [Space]
    public float frictionAcceleration;
    //[Space]
    //[Range(0f, 90f)] public float frontAngleAllowance = 30f;
	[Header("Camera fov")]
	public float fov;
	public float zOffset;
	[Header("Jump")]
    public float jumpHeight;
    public float reachTime;
    public float fallTime;
	public float coyoteTime;
    [Header("Gravity")]
    public float groundGravity;
    [Header("Slope")]
    public float maxSlopeAngle;
}