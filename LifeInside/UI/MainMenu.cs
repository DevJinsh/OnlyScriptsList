using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    float minRangeX;
    float maxRangeX;
    float currentPos;
    float speed = 0.05f;

    bool isTurn = false;
    private void Start()
    {
        minRangeX = -135;
        maxRangeX = -80;
        currentPos = 0;
    }

    private void FixedUpdate()
    {
        currentPos += Time.deltaTime;
        if(isTurn)
        {
            moveToLeft();
        }
        else
        {
            moveToRight();
        }
    }

    void moveToLeft()
    {
        transform.position = Vector3.Lerp(new Vector3(maxRangeX, transform.position.y, transform.position.z), new Vector3(minRangeX, transform.position.y, transform.position.z), (currentPos * speed));
        if((currentPos * speed) >= 1)
        {
            currentPos = 0;
            isTurn = false;
        }
    }
    void moveToRight()
    {
        transform.position = Vector3.Lerp(new Vector3(minRangeX, transform.position.y, transform.position.z), new Vector3(maxRangeX, transform.position.y, transform.position.z), (currentPos * speed));
        if ((currentPos * speed) >= 1)
        {
            currentPos = 0;
            isTurn = true;
        }
    }
}