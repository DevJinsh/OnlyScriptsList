using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerMovementManager playerMoveManager = other.GetComponent<PlayerMovementManager>();
            GameManager.Instance.playerSavePoint = transform.position;
            SaveSystem.SavePlayer(GameManager.Instance.playerSavePoint);
            Destroy(this);
        }
    }
}
