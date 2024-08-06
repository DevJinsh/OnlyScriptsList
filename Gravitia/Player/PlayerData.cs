using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] savePosition;
    //public int sceneNumber;

    public PlayerData (Vector3 savePos)
    {
        savePosition = new float[3];
        savePosition[0] = savePos.x;
        savePosition[1] = savePos.y;
        savePosition[2] = savePos.z;
    }
}
