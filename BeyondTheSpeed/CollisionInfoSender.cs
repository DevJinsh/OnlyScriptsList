using UnityEngine;

public class CollisionInfoSender : MonoBehaviour
{
    PlayerController player;

    private void Awake()
    {
        player = transform.root.GetComponent<PlayerController>();
        if (!name.Contains("@")) Debug.LogWarning(name + "�ݶ��̴� �̸� �տ� @�� �ٿ��ּ���.");
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
    /// �ݸ����� ��ü.
    /// </summary>
    public Collider sender;
    /// <summary>
    /// �ε��� ��� �ݶ��̴�.
    /// </summary>
    public Collider other;
    /// <summary>
    /// Enter, Stay, Exit�� ��������.
    /// </summary>
    public CollisionPhase phase;
}

public enum CollisionPhase
{
    Enter, Stay, Exit
}
