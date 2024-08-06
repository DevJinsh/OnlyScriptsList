
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.ReloadScene(other.gameObject);
            GameManager.Instance.hapticManager.RumbleGamePad(0.6f, 0.6f);
            //StartCoroutine(IWaitCoroutine(other.gameObject));
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Steam Stat
            SteamIntegration.AddStat("stat_deaths", 1);
            if (this.name.Substring(0,5) == "Thorn")
            {
                SteamIntegration.AddStat("stat_deaths_on_thorns", 1);
            }

            GameManager.Instance.ReloadScene(collision.gameObject);
            GameManager.Instance.hapticManager.RumbleGamePad(0.6f, 0.6f);
            //StartCoroutine(IWaitCoroutine(collision.gameObject));
        }
        if (collision.collider.CompareTag("InteractiveObj"))
        {
            if (!this.CompareTag("TurretBullet") && !collision.collider.TryGetComponent<NotActiveOnDeadLine>(out var NotActiveOnDeadLine))
            {
                if (collision.collider.TryGetComponent<ReSpawnObject>(out var obj))
                {
                    obj.RespawnObject();
                }
                if (collision.collider.TryGetComponent<DestructibleObject>(out var destructibleObject))
                {
                    destructibleObject.SetDestroyedObject();
                }
            }
        }
    }
}
