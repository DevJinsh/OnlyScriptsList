using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameObject : MonoBehaviour
{
    public List<GameObject> gameobjects;

    public void SetGameObjects(Vector3 respawnPos)
    {
        foreach(GameObject obj in gameobjects)
        {
            if (obj != null && obj.transform.position.x < respawnPos.x)
            {
                obj.SetActive(false);
            }
        }
    }
}
