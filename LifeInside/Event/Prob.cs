using System.Collections.Generic;
using UnityEngine;

public static class Prob
{

    public static int SelectEvent(List<float> eventProbabilities)
    {
        // 모든 확률의 총합을 계산합니다.
        float total = 0;
        foreach (float elem in eventProbabilities)
        {
            total += elem;
        }

        // 정규화: 각 확률을 총합으로 나눕니다.
        for (int i = 0; i < eventProbabilities.Count; i++)
        {
            eventProbabilities[i] /= total;
        }

        float randomPoint = Random.value;

        for (int i = 0; i < eventProbabilities.Count; i++)
        {
            if (randomPoint < eventProbabilities[i])
            {
                return i;
            }
            else
            {
                randomPoint -= eventProbabilities[i];
            }
        }

        Debug.Log("SelectEvent index is -1 | Prob Class");
        return -1;
    }

    public static int calcProb(List<Episode> eps)
    {
        List<float> probs = new List<float>();

        foreach(Episode ep in eps)
        {
            probs.Add(ep.probability);
            Debug.Log(ep);
        }

        Debug.Log($"probs.Count: {probs.Count}  | Prob Class");
        int selectedNum = SelectEvent(probs);

        if(selectedNum == -1){ return -1; }
        return selectedNum; 
    }
}