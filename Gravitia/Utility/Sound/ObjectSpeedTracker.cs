using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpeedTracker : MonoBehaviour
{
    private float lastYPosition;
    private float currentYPosition;

    private float lastXPosition;
    private float currentXPosition;

    public float speedThreshold = 5f;

    void Start()
    {
        lastYPosition = transform.position.y;
        lastXPosition = transform.position.x;
    }

    void Update()
    {
        //상하 트레일
        currentYPosition = transform.position.y;

        float displacementY = currentYPosition - lastYPosition;
        float speedY = Mathf.Abs(displacementY / Time.deltaTime);


        if (speedY > speedThreshold ) SoundManager.instance.SoundPlay("Lift", this.gameObject);
        else if (speedY >= speedThreshold + 1 && speedY <= speedThreshold + 20)  SoundManager.instance.SoundPlay("Lift2", this.gameObject);
        else if(speedY > speedThreshold + 20) SoundManager.instance.SoundPlay("Lift3", this.gameObject);



        lastYPosition = currentYPosition;

        //좌우 트레일
        currentXPosition = transform.position.x;
        float displacementX = currentXPosition - lastXPosition;
        float speedX = Mathf.Abs(displacementX / Time.deltaTime);

        if (speedX > speedThreshold && speedX < 25)
        {
            SoundManager.instance.SoundPlay("Lift", this.gameObject);
        }
        else if (speedX > 25)
        {
            SoundManager.instance.SoundPlay("Lift2", this.gameObject);
        }

        lastXPosition = currentXPosition;
    }
}
