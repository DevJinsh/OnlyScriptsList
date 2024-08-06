using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSound : MonoBehaviour
{

    [SerializeField] private float minPos = -1.31f;
    [SerializeField] private float maxPos = 6.66f;
    [SerializeField] private int value = 10;
    [SerializeField] private float calculateTime = 0.5f;

    [SerializeField] private bool isVertical = true;

    float deltaValue;
    public int CurrentValue { get; private set; }

    private void Start()
    {
        var distance = maxPos - minPos;
        deltaValue = distance / value;

        StartCoroutine(CheckRailPos());
    }

    IEnumerator CheckRailPos()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(calculateTime);

        yield return null;
        while (true)
        {
            yield return waitForSeconds;
            if (isVertical)
            {
                for (int i = 0; i < value; i++)
                {
                    var currentValue = transform.position.y;
                    
                    if (currentValue >= minPos + deltaValue * i)
                    {
                        CurrentValue = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < value; i++)
                {
                    var currentValue = transform.position.x;

                    if (currentValue >= minPos + deltaValue * i)
                    {
                        CurrentValue = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
