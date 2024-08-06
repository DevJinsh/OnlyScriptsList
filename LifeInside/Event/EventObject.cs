using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EventObject : MonoBehaviour
{
    Episode calcDefEp; // show info 에서 쓰임. default 이벤트만

    public string objName; // 외부에서 입력해넣음

    bool canShowInfo;

    public List<Episode> episodes;

    public float uiOffset;

    public bool isPositiveHappy;
    public bool isPositiveFatigue;
    public bool isPositiveMoney;
    public int sendTime;

    public bool isTouching;

    public void init(string objName_, List<Episode> episodes_)
    {
        episodes = GameManager.I.eventManager.sortByLocation(objName);
        objName = objName_;   // 마트
        episodes = episodes_;  // 마트 에피소드
        Debug.Log($"init EventObject {objName}  에피소드 수 {episodes.Count}");

        GameTime.hourChangedEvent += UpdateInteractionInfo;
    }


    public void ProcessEvent()
    {
        Debug.Log($" Process Event {objName} "); 

        List<Episode> epiByleveled = sortByLevelInEvtObj(episodes);
        //int selectedNo = Prob.calcProb(epiByleveled);
        List<Episode> sortEpi = epiByleveled.Where(item => item.defaultEpi == true).ToList();

        /*if     ( selectedNo > epiByleveled.Count ){ selectedNo = epiByleveled.Count-1;}
        else if( selectedNo == -1 ){ return; }

        Debug.Log($" selectedNo {selectedNo}    epiByleveled.Count  {epiByleveled.Count}");
        */
        //Episode selectedEp = epiByleveled[selectedNo];
        Episode selectedEp = sortEpi[0];
        if ( !CheckIfProcess() ){ return; }

        GameManager.I.uIController.uIEffect.EffectStart();

        GameManager.I.dialogueManager.PlayEvtDialogueFile(selectedEp.fileName);  //selectEp 와 calcDefEp 의 차이? 

        StartCoroutine(WaitEndDialog(selectedEp));
    }

    bool CheckIfProcess()
    {
        // 여기서 막기, 실행되면 안되는 이벤트. 디폴트 기준으로 막음
        // 금전 부족은, 현재 가진 돈을 기준으로 보여줌(default 이벤트).
        // 디폴트보다 돈이 많다면 실행은 가능. 돈이 더깎이면 0원. 덜깎이면 다행.
        if(calcDefEp == null){ 
            Debug.Log("calc Def Ep is null");
            return false; 
        }

        if(GameManager.I.money < calcDefEp.moneyCost){
            Debug.Log($" 돈 부족으로 실행 실패 | 현재돈  {GameManager.I.money}  필요돈  {calcDefEp.moneyCost}");
            return false;
        }
        if (21 - GameTime.curHour < calcDefEp.timeCost ) //남은 시간이 적으면 실행 불가 21인 이유 -> 21:00초부터 막아야 초단위 포함이기 때문
        {
            Debug.Log($"시간 부족으로 실행 실패 | 남은시간 {21 - GameTime.curHour}  필요시간 {calcDefEp.timeCost}");
            return false;
        }

        return true;
    }



    IEnumerator WaitEndDialog(Episode selectedEp)
    {
        // GameManager.I.eventManager.isCurDialogEnd 값이 true 가 된후 이후 코드 실행
        yield return new WaitUntil(() => GameManager.I.eventManager.isCurDialogEnd );

        GameTime.I.CostTime(selectedEp.timeCost);
        GameManager.I.AddCharacterResource(selectedEp.happyGain, selectedEp.fatigueGain, selectedEp.moneyGain);
        GameManager.I.SubCharacterResource(selectedEp.happyCost, selectedEp.fatigueCost, selectedEp.moneyCost);

        if( selectedEp.hasMoreEvent ){
            GameManager.I.uIController.selectEventWindow.GetComponent<SelectOption>().SetSubEvent(selectedEp);
            GameManager.I.uIController.selectEventWindow.SetActive(true); // 추가 이벤트 실행여부 파악창
        }

        Debug.Log("selectedEp.respawnPos: "+ selectedEp.respawnPos);
        // 병원 이벤트
        if ( selectedEp.respawnPos == "병원" )
        {
            Debug.Log("병원 이벤트 시작");
            if( GameManager.I.hospital != null){
                Debug.Log("병원위치에서 리스폰");
                GameManager.I.playerController.transform.position 
                    = new Vector3(GameManager.I.hospital.transform.position.x, GameManager.I.hospital.transform.position.y, 0);
            }
        }
    }

    public void ShowInfo()
    {
        GameManager.I.uIController.interactionInfo.SetActive(true);
    }
    public void HideInfo()
    {
        GameManager.I.uIController.interactionInfo.SetActive(false);
        resetCalcDefEp();
    }

    public void JudgeInfo()
    {
        Episode dEp = GetDefaultEp(this.objName);

        if( dEp != null)
        {
            canShowInfo = true;

            bool canAffordMoeny = dEp.moneyCost <=  GameManager.I.money;
            bool canAffordTime  = dEp.timeCost  >  (GameTime.endHour - GameTime.curHour - 1) ;
            GameManager.I.uIController.SetEpisodeInfo(dEp, canAffordMoeny, canAffordTime);
        }
        else
        {
            canShowInfo = false;
            GameManager.I.uIController.SetEmptyInfo(this.objName);

        }
    }
    Episode GetDefaultEp(string location)
    {
        int current_level = GameManager.I.playerController.level;
        //Debug.Log($"CalcInfo : {current_level}");
        List<Episode> sortLvlEp       = GameManager.I.eventManager.sortByLevel (current_level);
        List<Episode> sortLvLocaEp    = sortLvlEp.Where(   item => item.location   == location).ToList();
        List<Episode> sortLvLocaDefEp = sortLvLocaEp.Where(item => item.defaultEpi == true    ).ToList();

        // Debug.LogFormat("sorted Ep Level({0}) Loca({1}) Default({2})",sortLvlEp.Count,sortLvLocaEp.Count, sortLvLocaDefEp.Count);

        if (sortLvLocaDefEp.Count > 0)
        {
            calcDefEp = sortLvLocaDefEp[0];  // todo. 디폴트가 있다면 디폴트만 됨.
        }else{
            // 현재 default event가 없는 곳은 없다.
        }

        return calcDefEp;
    }

    public void UpdateInteractionInfo()
    {
        if( !isTouching ){ return; }
        if( !GameManager.I.uIController.interactionInfo.activeSelf ){ return; }
        
        JudgeInfo();
    }

    public void resetCalcDefEp()
    {
        calcDefEp = null;
    }


    List<Episode> sortByLevelInEvtObj(List<Episode> epis)
    {
        List<Episode> leveldepis = epis.Where(ep => ep.level == GameManager.I.playerController.level).ToList();

        
        foreach(Episode ep in leveldepis)
        {
            Debug.Log($"gameM level : {GameManager.I.playerController.level}  ep.level {ep.level}  ep {ep}" );
        }

        return leveldepis;
    }
}
