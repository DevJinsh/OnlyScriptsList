using UnityEngine;

[CreateAssetMenu(fileName = "BreakFallData", menuName = "ScriptableObjects/BreakFallData")]
public class BreakFallData : ScriptableObject
{
	[Header("Detection")]
	public float validTime;
    [Header("Movement")]
    public float breakFallFriction;
    [Space]
    [Header("SpinCamera")]
    public float cameraSpinTime;
    [Space]
	[Header("BreakFallBooster")]
	public float yVelocityThreshold;
	public float boostPower;
}
