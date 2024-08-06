using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reaper : MonoBehaviour
{
    private Coroutine disableReaper;
    void Start()
    {
        gameObject.SetActive(false);
    }

    void ChangeEffectReaper(float deathProb)
    {
        //������ ����
        Color imageColor = GetComponent<Image>().color;
        imageColor.a = deathProb / 20f;
        GetComponent<Image>().color = imageColor;
        //ũ�� ����
        GetComponent<RectTransform>().sizeDelta *= 1 + deathProb / 20f;
    }

    public void EnableReaper() // 경고하는 사신 연출, 4일이후 매일 지날떄
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        GameManager.I.uIController.uIEffect.EffectStart();
        float deathProb = GameManager.I.playerController.deathProb;
        
        if (deathProb < 3f) // 1단계
        {
            GameManager.I.dialogueManager.PlayEvtDialogueFile("Reaper");
        }
        else if (deathProb < 8f) // 2단계 
        {
            GameManager.I.dialogueManager.PlayEvtDialogueFile("Reaper");
            ChangeEffectReaper(deathProb);
        }
        else if (deathProb < 15f) // 3단계 (경고)
        {
            GameManager.I.dialogueManager.PlayEvtDialogueFile("Reaper");
            ChangeEffectReaper(deathProb);
        }
        else // 4단계 (위협)
        {
            GameManager.I.dialogueManager.PlayEvtDialogueFile("Reaper");
            ChangeEffectReaper(deathProb);
        }
        if (disableReaper != null) StopCoroutine(disableReaper);
        disableReaper = StartCoroutine(DisableReaper());
    }

    public void EndingSceneReaper() // 사망연출 사신, Death()에서 콜. 각 죽음 대사 치고 넘어옴.
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        GetComponent<Image>().color = Color.white;
        GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 500f);

        if (disableReaper != null) StopCoroutine(disableReaper);
        disableReaper = StartCoroutine(DisableReaper());
    }

    IEnumerator DisableReaper()
    {
        yield return new WaitUntil(() => GameManager.I.eventManager.isCurDialogEnd);
        Time.timeScale = 1f;
        gameObject.SetActive(false);

        GameManager.I.DeathLast();

    }


}
