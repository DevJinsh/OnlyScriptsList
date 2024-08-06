using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfinePosition : MonoBehaviour
{
    public bool zAxis;
    public float fixedZValue;
    private void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, fixedZValue);
    }
}
