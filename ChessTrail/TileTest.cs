using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTest : MonoBehaviour
{
    public Transform parent;
    public GameObject cell;
    public int cellcount;
    private void Start()
    {
        for (int i = 0; i < cellcount * cellcount; i++)
        {
            Instantiate(cell, parent);
        }
    }
}
