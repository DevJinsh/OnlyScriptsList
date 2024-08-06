using System.Collections;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    private float currentAnchorePosY;
    private float goalAnchorePosY;
    private float currentSecond;

    public float endSecond;

    private Coroutine coroutine;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void init()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.up * -GetComponent<RectTransform>().rect.height;
    }
    public void EffectStart()
    {
        GameManager.I.inputManager.LockInput();
        gameObject.SetActive(true);
        init();
        goalAnchorePosY = 0;
        currentAnchorePosY = GetComponent<RectTransform>().anchoredPosition.y;
        Time.timeScale = 0f;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Effecting(currentAnchorePosY, goalAnchorePosY, false));
    }

    public void EffectEnd()
    {
        GameManager.I.inputManager.UnLockInput();
        goalAnchorePosY = -GetComponent<RectTransform>().rect.height;
        currentAnchorePosY = GetComponent<RectTransform>().anchoredPosition.y;
        Time.timeScale = 1f;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Effecting(currentAnchorePosY, goalAnchorePosY, true));
    }
    IEnumerator Effecting(float start, float end, bool isEnding)
    {
        currentSecond = 0f;
        while (currentSecond < endSecond)
        {
            currentSecond += Time.fixedDeltaTime;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Lerp(start, end, currentSecond / endSecond));
            yield return null;
        }
        if(isEnding)
        {
            if(!GameManager.I.playerController.isDeath){
                gameObject.SetActive(false);
            }
        }
    }


}
