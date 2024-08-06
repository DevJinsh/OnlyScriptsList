using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagContact : MonoBehaviour
{
    public string[] contactedObjectTags;
    public bool[] isContactedArray;

    private void Awake()
    {
        for (int i = 0; i < isContactedArray.Length; i++)
        {
            isContactedArray[i] = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        int i = 0;
        foreach (string tag in contactedObjectTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                isContactedArray[i] = true;
            }
            i++;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        int i = 0;
        foreach (string tag in contactedObjectTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                isContactedArray[i] = false;
            }
            i++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int i = 0;
        foreach (string tag in contactedObjectTags)
        {
            if (other.CompareTag(tag))
            {
                isContactedArray[i] = true;
            }
            i++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int i = 0;
        foreach (string tag in contactedObjectTags)
        {
            if (other.CompareTag(tag))
            {
                isContactedArray[i] = false;
            }
            i++;
        }
    }

}
