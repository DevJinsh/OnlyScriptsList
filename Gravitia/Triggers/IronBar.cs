using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IronBar : MonoBehaviour
{
    public LayerMask collisionMask;
    public GameObject toChangeObject;

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & collisionMask) != 0)
        {
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<BoxCollider>());
            toChangeObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            foreach (MeshRenderer mesh in toChangeObject.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.material.DOFade(0, 4f).OnComplete(() => Destroy(gameObject));
            }
        }
    }
}
