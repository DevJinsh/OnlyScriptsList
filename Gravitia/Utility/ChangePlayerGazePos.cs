using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerGazePos : MonoBehaviour
{
    public GameObject gazeTarget;
    public Vector3 pos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gazeTarget.transform.position = pos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gazeTarget.transform.position = pos;
        }
    }

    public IEnumerator AdjustYCouroutine(float startY, float endY, float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float y = Mathf.Lerp(startY, endY, (currentTime / time) * (currentTime / time) * (currentTime / time) * (currentTime / time));
            gazeTarget.transform.position = new Vector3(gazeTarget.transform.position.x, y, gazeTarget.transform.position.z);
            yield return null;
        }
    }
}
