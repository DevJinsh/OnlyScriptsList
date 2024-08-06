using UnityEngine;
[CreateAssetMenu(fileName = "ClimbUpData", menuName = "ScriptableObjects/ClimbUpData")]
public class ClimpUpData : ScriptableObject
{
    [Header("Climbing")]
    public float climbOffset;
    public float minClimbSpeed;
    public float lowClimbOffset;
    public float highClimbOffset;
    public float climbForwardOffest;
    public int maxOverlapBoxOffset;
	[Range(0, 1)]
    public float decreaseRate;
    public float lookupStartTime;
    public float lookupDuration;
    public float lookdownDuration;
    [Header("State")]
    public bool canClimb;
    public float maxVelocity;
}
