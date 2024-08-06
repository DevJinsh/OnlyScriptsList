using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperController : MonoBehaviour
{
    public GameObject target;

    float distance;
    float range = 3f;
    float smooth = 0.0001f;
    Coroutine moveCoroutine;

    private void FixedUpdate()
    {
        distance =  Vector2.Distance(target.transform.position, transform.position);
        if(distance > range)
        {
            if (moveCoroutine == null)
            {
                StartCoroutine(MoveToTarget());
            }
        }
        //투명도 조절
        Color imageColor = GetComponent<SpriteRenderer>().color;
        imageColor.a = GameManager.I.playerController.deathProb / 20f;
        GetComponent<SpriteRenderer>().color = imageColor;
    }

    IEnumerator MoveToTarget()
    {
        float current_Time = 0f;
        while (distance >= range)
        {
            yield return null;
            current_Time += Time.fixedDeltaTime;
            transform.position = Vector2.Lerp(transform.position, target.transform.position, distance * smooth);
        }
    }
}
