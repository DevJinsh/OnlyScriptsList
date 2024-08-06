using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreatmentDestruction : MonoBehaviour
{
    int crumbleNum;

    private void Awake()
    {
        crumbleNum = (int)(transform.GetChild(0).childCount * 0.1f);
    }
    public void TreatDestruction(bool enable)
    {
        if (enable)
        {
            TreatBeforeDestruction();
        }
        else
        {
            TreatAfterDestruction();
        }
    }
    private void TreatBeforeDestruction()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    private void TreatAfterDestruction()
    {
        Transform child = transform.GetChild(0);
        int childCount = child.childCount;
        for (int index = 0; index < childCount; index++)
        {
            child.GetChild(index).gameObject.SetActive(true);
        }
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void CrumbleObject()
    {
        int maxCount = transform.GetChild(0).childCount;

        for (int i = 0; i < crumbleNum; i++)
        {
            int randomNum = Random.Range(0, maxCount);
            Transform obj = transform.GetChild(0).GetChild(randomNum);
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().useGravity = true;
            obj.GetComponent<BoxCollider>().enabled = true;
            obj.GetComponent<MeshRenderer>().material.DOFade(0, 2f).OnComplete(() => obj.gameObject.SetActive(false));
        }
    }
}
