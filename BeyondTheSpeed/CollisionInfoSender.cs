using UnityEngine;

public class CollisionInfoSender : MonoBehaviour
{
    PlayerController player;

    private void Awake()
    {
        player = transform.root.GetComponent<PlayerController>();
        if (!name.Contains("@")) Debug.LogWarning(name + "콜라이더 이름 앞에 @를 붙여주세요.");
    }
    private void OnTriggerEnter(Collider other)
    {
        CollisionInfo _info = new CollisionInfo();
        _info.sender = GetComponent<Collider>();
        _info.other = other;
        _info.phase = CollisionPhase.Enter;
        player.PassCollisionInfo(_info);
    }
    private void OnTriggerStay(Collider other)
    {
        CollisionInfo _info = new CollisionInfo();
        _info.sender = GetComponent<Collider>();
        _info.other = other;
        _info.phase = CollisionPhase.Stay;
        player.PassCollisionInfo(_info);
    }
    private void OnTriggerExit(Collider other)
    {
        CollisionInfo _info = new CollisionInfo();
        _info.sender = GetComponent<Collider>();
        _info.other = other;
        _info.phase = CollisionPhase.Exit;
        player.PassCollisionInfo(_info);
    }
}

public struct CollisionInfo
{
    /// <summary>
    /// 콜리션의 주체.
    /// </summary>
    public Collider sender;
    /// <summary>
    /// 부딪힌 상대 콜라이더.
    /// </summary>
    public Collider other;
    /// <summary>
    /// Enter, Stay, Exit중 무엇인지.
    /// </summary>
    public CollisionPhase phase;
}

public enum CollisionPhase
{
    Enter, Stay, Exit
}
