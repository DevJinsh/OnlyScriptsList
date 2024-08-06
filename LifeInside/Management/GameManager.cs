using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public  EventManager     eventManager;
    [HideInInspector] public  CutSceneManager  cutSceneManager;
    [HideInInspector] public  PlayerController playerController;

    [HideInInspector] public DialogueManager dialogueManager;

    [HideInInspector] public Inputs inputManager;

    [HideInInspector] public UIController uIController;

    public float happiness = 100;
    public float fatigue   = 100;
    public float money     = 50000;

    public GameObject hospital;

    void Start()
    {
        cutSceneManager  = FindAnyObjectByType<CutSceneManager>();// GetComponent<CutSceneManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        uIController     = FindAnyObjectByType<UIController>();
        eventManager     = FindAnyObjectByType<EventManager>();
        dialogueManager  = FindObjectOfType<DialogueManager>();
        inputManager     = FindObjectOfType<Inputs>();

        if(eventManager == null){ Debug.LogWarning("eventManager is null"); return; }


        CSVReader.I.ReadCsvIntoEpisodes(eventManager.episodeAll);
        eventManager.SetEvtObj(); // 반드시 ReadCsvIntoEpisodes() 이후에 call할 것.

        SetHappinessUI( (int)happiness );
        SetFatigueUI(   (int)fatigue   );
        SetMoneyText(   (int)money     );

        
        Debug.Log("Tutorial Start | LOCK input, Stop time.");
        inputManager.LockInput();
        GameTime.I.StopTime();
    }


    public void StartMainGame()
    {
        Debug.Log("Start MainGame");
        inputManager.UnLockInput();
        GameTime.I.FlowTime();
    }

    public bool CheckIfDeath()
    {
        Debug.Log($"CheckIfDeath |  행복 {happiness}  피로 {fatigue}  돌연사확률 {playerController.deathProb}");

        if( happiness <=   0 ){ Death("노잼사"); return true; }
        if( fatigue   >= 200 ){ Death("과로사"); return true; }

        if (Random.Range(0,100) <= playerController.deathProb) // 죽음 확률에 따라 돌연사 or 호상
        {
            if (happiness > 150 && fatigue < 100)
            {
                Death("호상");
                return true;
            }
            else
            {
                Death("돌연사");
                return true;
            }
        }

        return false; // 다음날로 게임 진행
    }


    void Death(string reason)
    {
        playerController.isDeath = true;

        uIController.uIEffect.EffectStart();
        
        string fileName;
        Debug.Log( reason );
        switch(reason)
        {
            case "과로사":
                fileName = eventManager.GetCutSceneFileByName(reason);
                dialogueManager.PlayEvtDialogueFile("Death_2");
                //SceneManager.LoadScene("CityMap");
                break;
            case "노잼사":
                fileName = eventManager.GetCutSceneFileByName(reason);
                dialogueManager.PlayEvtDialogueFile("Death_1");
                //SceneManager.LoadScene("CityMap");
                break;
            case "돌연사":
                fileName = eventManager.GetCutSceneFileByName(reason);
                dialogueManager.PlayEvtDialogueFile("Death_4");
                //SceneManager.LoadScene("CityMap");
                break;
            case "호상":
                fileName = eventManager.GetCutSceneFileByName(reason);
                dialogueManager.PlayEvtDialogueFile("Death_3");
                //SceneManager.LoadScene("CityMap");
                break;
            default:
            break;
        }
        uIController.reaper.EndingSceneReaper();

    }

    // public void GameOver()
    // {
    //     dialogueManager.PlayEvtDialogueFile("Ending");
    //     if(eventManager.isCurDialogEnd)
    //     {
    //         Application.Quit();
    //     }
    // }

    public void AddCharacterResource(int _happy, int _fatigue, int _money)
    {
        //Debug.Log(" 행복 +"+ _happy +" 피로 +"+ _fatigue +" 돈 +" + _money);


        happiness += _happy;
        fatigue   += _fatigue;
        money     += _money;


        if( happiness > 200 ){ happiness = 200; }
        if( fatigue   > 200 ){ fatigue   = 200; }


        SetHappinessUI( (int)happiness );
        SetFatigueUI(   (int)fatigue   );
        SetMoneyText(   (int)money );
    }

    public void SubCharacterResource(int _happy, int _fatigue, int _money)
    {
        //Debug.Log(" 행복 -"+ _happy +" 피로 -"+ _fatigue +" 돈 -" + _money);
        
        happiness -= _happy;
        fatigue   -= _fatigue;
        money     -= _money;

        if( happiness < 0 ){ happiness = 0; }
        if( fatigue   < 0 ){ fatigue   = 0; }
        if( money     < 0 ){ money     = 0; }

        SetHappinessUI( (int)happiness );
        SetFatigueUI(   (int)fatigue );
        SetMoneyText(   (int)money );
    }

    public void SetHappinessUI(int intValue)
    {
        if     ( intValue < 0   ){ intValue = 0;   }
        else if( intValue > 200 ){ intValue = 200; }

        //Debug.Log("Set Happiness Value : " + intValue);

        float floatValue = ((float)intValue) / 200f;
        uIController.SetHappySlider( floatValue ); // value 0 ~ 1f
    }

    public void SetFatigueUI(int intValue)
    {
        if     ( intValue < 0   ){ intValue = 0;   }
        else if( intValue > 200 ){ intValue = 200; }

        //Debug.Log("Set Fatigue Value : " + intValue);
    

        float floatValue = ((float)intValue) / 200f;
        uIController.SetFatigueSlider( floatValue ); // value 0 ~ 1f
    }

    public void SetMoneyText(int money_)
    {
        //Debug.Log("Set Moeny Value : " + money_);

        uIController.SetMoneyText( money_ ); // value 0 ~ 1f

    }


    public void CutSceneStart(string eventfile)
    {
        inputManager.LockInput();
    }

    public void CutSceneEnd()
    {
        inputManager.UnLockInput();
    }
 
    public void Tutorial(string name)
    {
        
        string fileName = eventManager.GetCutSceneFileByName(name);
        uIController.uIEffect.EffectStart();
        dialogueManager.PlayEvtDialogueFile(fileName);
        
    }

    public void DeathLast()
    {
        if(playerController.isDeath)
        {
            dialogueManager.PlayEvtDialogueFile("Ending");
            StartCoroutine(LastEnding());
        }
    }
    IEnumerator LastEnding()
    {
        yield return new WaitUntil(() => eventManager.isCurDialogEnd);
        Application.Quit();

    }
}
