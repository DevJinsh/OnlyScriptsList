using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaffoldingObject : MonoBehaviour
{
    public float rotateAngle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.transform.DORotate(new Vector3(0, 0, rotateAngle), 1f, RotateMode.Fast);
        }
    }
}
