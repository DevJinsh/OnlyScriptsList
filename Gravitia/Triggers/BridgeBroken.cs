using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBroken : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject roadBase;
    public GameObject roadBroken;
    public GameObject smoke;
    public float brokeForce = 1f;
    
    public void BridgeBroke()
    {
        roadBase.SetActive(false);
        roadBroken.SetActive(true);
        foreach (Rigidbody rb in roadBroken.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(new Vector3(0, -1, 0) * brokeForce, ForceMode.Impulse);
        }
        smoke.SetActive(true);
        StartCoroutine(ChangeLayer());
    }

    IEnumerator ChangeLayer()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < roadBroken.transform.childCount; i++)
        {
            roadBroken.transform.GetChild(i).gameObject.layer = ReturnQuotient(LayerMask.GetMask("IgnoreGravityZone"));
        }
    }
    int ReturnQuotient(int value)
    {
        int returnNum = 0;
        while (value / 2 != 0)
        {
            returnNum++;
            value = value / 2;
        }
        return returnNum;
    }
}
