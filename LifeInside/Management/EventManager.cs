using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public List<EventObject> eventObjs  = new List<EventObject>(); // 마트, 편의점, 학교...
    public List<Episode>     episodeAll = new List<Episode>();


    public bool isCurDialogEnd = false;

    public EventManager()
    {
        Debug.Log("Create EventManager");
    }

    public void SetEvtObj() // TODO: 개선요. 인스펙터와 이거 중 하나만 확인하도록.
    {
        Debug.Log("Set EvtObj");
        eventObjs[0].init("어린이집",   sortByLocation("어린이집") );
        eventObjs[1].init("키즈카페",   sortByLocation("키즈카페") );
        eventObjs[2].init("문화센터",   sortByLocation("문화센터") );
        eventObjs[3].init("피시방",     sortByLocation("피시방")   );
        eventObjs[4].init("초등학교",   sortByLocation("초등학교") );
        eventObjs[5].init("헌팅 포차",  sortByLocation("헌팅 포차"));
        eventObjs[6].init("마트",       sortByLocation("마트")     );
        eventObjs[7].init("편의점",     sortByLocation("편의점")   );
        eventObjs[8].init("회사",       sortByLocation("회사")     );
        eventObjs[9].init("헬스장",     sortByLocation("헬스장")   );
        eventObjs[10].init("노인정",    sortByLocation("노인정")   );
        eventObjs[11].init("골프장",    sortByLocation("골프장")   );
        eventObjs[12].init("콜라텍",    sortByLocation("콜라텍")   );
        eventObjs[13].init("벤치",      sortByLocation("벤치")     );
        eventObjs[14].init("벤치",      sortByLocation("벤치")     );
        eventObjs[15].init("벤치",      sortByLocation("벤치")    );
        eventObjs[16].init("병원",      sortByLocation("병원")    ); // 특수
        eventObjs[17].init("자판기",    sortByLocation("자판기")  );
        eventObjs[18].init("자판기",    sortByLocation("자판기")  );
        eventObjs[19].init("자판기",    sortByLocation("자판기")  );
        
    }


    public void EventStart(EventObject eventObj)
    {
        if(eventObj == null){ 
            Debug.Log(" eventObj is null");
            return;
        }

        Debug.Log("EventStart "  );
        Debug.Log("eventObj "  + eventObj?.objName);

        eventObj.ProcessEvent();
    }


    public List<Episode> sortByLocation(string location_)
    {
        List<Episode> epSorted = episodeAll.Where(item => item.location == location_).ToList();

        //Debug.Log("sortByLocation " + epSorted.Count);


        return epSorted;
    }

    public List<Episode> sortByLevel(int level_)
    {
        List<Episode> epSorted = episodeAll.Where(item => item.level == level_).ToList();
        //Debug.Log("sortByLevel " +epSorted.Count);

        return epSorted;
    }

    public List<Episode> sortByDefault()
    {
        List<Episode> epSorted = episodeAll.Where(item => item.defaultEpi == true).ToList();

        //Debug.Log("sortByDefault " + epSorted.Count);


        return epSorted;
    }

    public string GetCutSceneFileByName(string _name)
    {


        Debug.Log("GetCutSceneFileByName " + _name);

        // if (evtNameFileDic.ContainsKey(_name))
        // {
        //     return evtNameFileDic[_name];
        // }
        // else
        // {
        //     return null;
        // }
        return null;
    }

    public void StartAddEvent(Episode selectedEp)
    {
        GameManager.I.dialogueManager.PlayEvtDialogueFile(selectedEp.fileName);  //selectEp �� calcDefEp �� ����? 
        StartCoroutine(WaitEndDialogAddEvent(selectedEp));
    }

    IEnumerator WaitEndDialogAddEvent(Episode selectedEp)
    {
        // GameManager.I.eventManager.isCurDialogEnd 값이 true 가 된후 이후 코드 실행
        yield return new WaitUntil(() => GameManager.I.eventManager.isCurDialogEnd);

        GameTime.I.CostTime(selectedEp.timeCost);
        GameManager.I.AddCharacterResource(selectedEp.happyGain, selectedEp.fatigueGain, selectedEp.moneyGain);
        GameManager.I.SubCharacterResource(selectedEp.happyCost, selectedEp.fatigueCost, selectedEp.moneyCost);

        Debug.Log("selectedEp.respawnPos: " + selectedEp.respawnPos);
        // 병원 이벤트
        if (selectedEp.respawnPos == "병원")
        {
            Debug.Log("병원 이벤트 시작");
            if (GameManager.I.hospital != null)
            {
                Debug.Log("병원위치에서 리스폰");
                GameManager.I.playerController.transform.position
                    = new Vector3(GameManager.I.hospital.transform.position.x, GameManager.I.hospital.transform.position.y, 0);
            }
        }
    }
}
