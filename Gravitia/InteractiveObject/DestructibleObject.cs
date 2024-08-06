using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private string SoundMaterial;
    public void SetRespawnObject()
    {
        SetObject(true);
    }
    public void SetDestroyedObject()
    {
        SetObject(false);
        //Steam Stat
        SteamIntegration.AddStat("stat_trash_bags_destroyed", 1);
    }
    private void SetObject(bool enable)
    {
        SetOriginBox(enable);
        SetDestroyedBox(!enable);
    }
    private void SetOriginBox(bool enable)
    {
        GetComponent<Rigidbody>().isKinematic = !enable;
        GetComponent<Rigidbody>().useGravity = enable;
        GetComponent<Collider>().enabled = enable;
    }
    private void SetDestroyedBox(bool enable)
    {
        if (TryGetComponent<TreatmentDestruction>(out var treatmentDestruction))
        {
            treatmentDestruction.TreatDestruction(enable);
            SoundManager.instance.Destruct(SoundMaterial, this.gameObject);
        }
        foreach (Rigidbody rb in transform.GetChild(0).GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = !enable;
            rb.useGravity = enable;
        }
        foreach (BoxCollider boxCollider in transform.GetChild(0).GetComponentsInChildren<BoxCollider>())
        {
            boxCollider.enabled = enable;
        }
        foreach (Transform tf in transform.GetChild(0).GetComponentsInChildren<Transform>())
        {
            if (tf.name == transform.name)
                continue;
            if (tf.TryGetComponent<InitialTransform>(out var initialTransform))
            {
                initialTransform.ResetPosAndRotation();
            }
            else
            {
                tf.localPosition = new Vector3(0, 0, 0);
                tf.localRotation = new Quaternion(0, 0, 0, 1);
            }
        }
    }

    public void FadeOut(float time)
    {
        foreach (MeshRenderer mesh in transform.GetChild(0).GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.DOFade(0, time);
        }
    }

    public void FadeIn(float time)
    {
        foreach (MeshRenderer mesh in transform.GetChild(0).GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.DOFade(1, 0);
        }
    }
}
