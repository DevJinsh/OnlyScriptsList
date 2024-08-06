using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChargeGauge : MonoBehaviour
{
    [SerializeField] Transform player;
    private Image chargeGaugeImage;
    public Gradient color;

    private void Start()
    {
        chargeGaugeImage = this.transform.GetComponent<Image>();
    }
    private void Update()
    {
        RotateGauge();
    }
    public void SetGauge(float amount)
    {
        chargeGaugeImage.fillAmount = amount;
        chargeGaugeImage.color = color.Evaluate(amount);
    }
    private void RotateGauge()
    {
        if (player.rotation.eulerAngles.y == 90f)
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
