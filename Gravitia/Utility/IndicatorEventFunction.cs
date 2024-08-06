using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEventFunction : MonoBehaviour
{
    [SerializeField] Indicator indicator;

    public void EnableIndicator()
    {
        indicator.gameObject.SetActive(true);
    }

    public void DisableIndicator()
    {
        indicator.gameObject.SetActive(false);
    }
}
