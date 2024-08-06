using UnityEngine;

[CreateAssetMenu(fileName = "SlidingData", menuName = "ScriptableObjects/SlidingData")]
public class SlidingData : ScriptableObject
{
    [Header("Sliding")]
    public float slidingAcceleration;
    public float slidingFriction;
    public float slidingUpdateFriction;
    public float slidingTime;
    public float slidingCoolTime;
    public Vector3 shrinkedColliderCenter;
    public float shrinkedColliderHeight;
    [Space]
    [Header("Camera")]
    public Vector3 cameraHeight;
    public float cameraMoveTime;
}
